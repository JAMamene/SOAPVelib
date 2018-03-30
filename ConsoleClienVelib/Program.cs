using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleClienVelib
{
    class Program
    {

        private static String Present (VelibLibrary.Station station)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("Station n°= ").Append(station.Number).AppendLine();
            builder.Append(station.Name).Append(", ").Append(station.Address).Append(", ").AppendLine(station.City);
            builder.Append("Number of stands : ").Append(station.TotalStands).Append(", Number of available bikes : ").Append(station.AvailableStands).AppendLine();
            return builder.ToString();
        }

        static void Main(string[] args)
        {
            VelibLibrary.VelibClient client = new VelibLibrary.VelibClient();
            List<String> cities = client.GetCities().ToList<String>();
            Console.WriteLine("Available Commands are help, cities, stations, exit");
            String cmd = "";
            while (cmd.ToLower() != "exit")
            {
                Console.WriteLine("Type a command");
                cmd = Console.ReadLine();
                switch (cmd.ToLower())
                {
                    case "help":
                        Console.WriteLine("Commands available are :\n" +
                            "help : print help\n" +
                            "cities : print all valid cities\n" +
                            "stations : print all stations of a city passed as parameter\n" +
                            "exit : exit the program");
                        break;

                    case "cities":
                        Console.WriteLine("Availables cities are");
                        foreach (String c in client.GetCities().ToList())
                        {
                            Console.Write(c + ", ");
                        }
                        Console.WriteLine();
                        break;

                    case "stations":
                        Console.WriteLine("Enter a city");
                        string city = Console.ReadLine();
                        IList<VelibLibrary.Station> stations = client.GetStations(city);
                        if (stations.Any())
                        {
                            foreach (VelibLibrary.Station s in stations)
                            {
                                Console.WriteLine(Present(s));
                            }
                        }
                        else
                        {
                            Console.WriteLine("No stations found in the city provided");
                        }
                        break;

                    case "exit":
                        break;
              
                    default:
                        Console.WriteLine("Invalid command");
                        break;
                }
            }
        }
    }
}
