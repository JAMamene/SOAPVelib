using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;
using System;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Linq;

namespace VelibLibrary
{
    public class Velib : IVelib, IMonitoring
    {
        private WebRequest request;
        private const string API_KEY = "apiKey=f7cc7565a9fc3788700377f355edf4077c6d56a6";
        private static MemoryCache cachedStations = new MemoryCache("stations");
        private static MemoryCache cachedCities = new MemoryCache("cities");
        private static string cityCacheName = "cities";
        private static IList<Request> requests = new List<Request>();
        private static IList<long> execTimes = new List<long>();

        public async Task<IList<string>> GetCitiesAsync()
        {
            System.Diagnostics.Stopwatch watch = System.Diagnostics.Stopwatch.StartNew();
            var elapsedMs = watch.ElapsedMilliseconds;
            if (cachedCities.Contains(cityCacheName))
            {
                requests.Add(new Request(true));
                watch.Stop();
                execTimes.Add(watch.ElapsedMilliseconds);
                return cachedCities.Get(cityCacheName) as List<string>;
            }
            else
            {
                request = WebRequest.Create("https://api.jcdecaux.com/vls/v1/contracts?" + API_KEY);
                WebResponse response = await request.GetResponseAsync();
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                JArray jsonResponse = JArray.Parse(responseFromServer);
                IList<string> cities = new List<string>();
                foreach (JObject jo in jsonResponse)
                {
                    cities.Add(jo.GetValue("name").ToString());
                }
                cachedStations.Set(cityCacheName, cities, DateTime.Now.AddMinutes(60));
                requests.Add(new Request(false));
                watch.Stop();
                execTimes.Add(watch.ElapsedMilliseconds);
                return cities;
            }
        }

        public async Task<IList<Station>> GetStationsAsync(string city)
        {
            System.Diagnostics.Stopwatch watch = System.Diagnostics.Stopwatch.StartNew();
            if (cachedStations.Contains(city))
            {
                requests.Add(new Request(true));
                watch.Stop();
                execTimes.Add(watch.ElapsedMilliseconds);
                return cachedStations.Get(city) as List<Station>;
            }
            else
            {
                request = WebRequest.Create("https://api.jcdecaux.com/vls/v1/stations?contract=" + city + "&apiKey=f7cc7565a9fc3788700377f355edf4077c6d56a6");
                WebResponse response = await request.GetResponseAsync();
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                JArray jsonResponse = JArray.Parse(responseFromServer);
                IList<Station> stations = new List<Station>();
                foreach (JObject jo in jsonResponse)
                {
                    Station s = new Station();
                    s.Number = Int32.Parse(jo.GetValue("number").ToString());
                    s.Name = jo.GetValue("name").ToString();
                    s.City = jo.GetValue("contract_name").ToString();
                    s.Address = jo.GetValue("address").ToString();
                    s.Status = jo.GetValue("status").ToString();
                    s.TotalStands = Int32.Parse(jo.GetValue("bike_stands").ToString());
                    s.AvailableStands = Int32.Parse(jo.GetValue("available_bikes").ToString());
                    s.Latitude = Double.Parse(jo["position"]["lat"].ToString());
                    s.Longitude = Double.Parse(jo["position"]["lng"].ToString());
                    stations.Add(s);
                }
                cachedStations.Set(city, stations, DateTime.Now.AddMinutes(3));
                requests.Add(new Request(false));
                watch.Stop();
                execTimes.Add(watch.ElapsedMilliseconds);
                return stations;
            }
        }

        public RequestInfo GetWSRequests()
        {
            RequestInfo requestInfo = new RequestInfo
            {
                Cached = requests.Count(r => r.IsCached()),
                Normal = requests.Count(r => !r.IsCached())
            };
            return requestInfo;
        }

        public RequestInfo GetWSRequestsTimeSpan(DateTime beg, DateTime end)
        {
            RequestInfo requestInfo = new RequestInfo
            {
                Cached = requests.Count(r => r.IsCached() && (end - r.GetDate()).TotalMinutes >= 0 && (beg - r.GetDate()).TotalMinutes < 0),
                Normal = requests.Count(r => !r.IsCached() && (end - r.GetDate()).TotalMinutes >= 0 && (beg - r.GetDate()).TotalMinutes < 0)
            };
            return requestInfo;
        }

        public Metrics GetMetrics()
        {
            Metrics metrics;
            if (execTimes.Any())
            {
                metrics = new Metrics
                {
                    Min = execTimes.Min(),
                    Avg = (long)execTimes.Average(),
                    Max = execTimes.Max()
                };
            }
            else
            {
                metrics = new Metrics
                {
                    Min = 0,
                    Avg = 0,
                    Max = 0
                };
            }
            return metrics;
        }
    }

    public class Request
    {
        private bool cached;
        private DateTime time;

        public Request(bool cached)
        {
            this.cached = cached;
            this.time = DateTime.Now;
        }

        public bool IsCached()
        {
            return cached;
        }

        public DateTime GetDate()
        {
            return time;
        }
    }
}

