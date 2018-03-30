using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GUIClienVelib
{

    public partial class MainWindow : Window
    {

        private IList<string> cities;
        private VelibLibrary.VelibClient client;
        private IDictionary<string, VelibLibrary.Station> stationInfo;
        private const string API_KEY = "&key=AIzaSyCRNzbeRYYqg_32_1q7C9FFNN4Fh9MGabk";

        public void loadImage(double latitude, double longitude)
        {
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri("https://maps.googleapis.com/maps/api/staticmap?center=" + latitude + "," + longitude +
                "&zoom=15&size=165x165&maptype=roadmap&markers=color:blue%7Clabel:S%7C" + latitude + "," + longitude, UriKind.RelativeOrAbsolute);
            bi.EndInit();
            StationImage.Source = bi;
        }


        private static String Present(VelibLibrary.Station station)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("Station n°= ").Append(station.Number).AppendLine();
            builder.AppendLine(station.Name).AppendLine(station.Address).AppendLine(station.City);
            builder.Append("Number of stands : ").Append(station.TotalStands).AppendLine().Append("Number of available bikes : ").Append(station.AvailableStands).AppendLine();
            return builder.ToString();
        }

        public MainWindow()
        {
            stationInfo = new Dictionary<string, VelibLibrary.Station>();
            InitializeComponent();
            client = new VelibLibrary.VelibClient();
            cities = client.GetCities().ToList();
            if (!cities.Any())
            {
                Cities.Items.Add(new ListViewItem { Content = "No cities found" });
            }
            else
            {
                foreach (string c in cities)
                {
                    Cities.Items.Add(new ListViewItem { Content = c });
                }
            }
        }


        async void RetrieveStations(object sender, SelectionChangedEventArgs args)
        {
            Stations.Items.Clear();
            stationInfo.Clear();
            StationImage.Source = new BitmapImage();
            ListViewItem selected = ((sender as ListView).SelectedItem as ListViewItem);
            IList<VelibLibrary.Station> stations = await client.GetStationsAsync(selected.Content.ToString());
            if (!stations.Any())
            {
                Stations.Items.Add(new ListViewItem { Content = "No stations found" });
            }
            else
            {
                foreach (VelibLibrary.Station s in stations)
                {
                    stationInfo.Add(s.Name, s);
                    Stations.Items.Add(new ListViewItem { Content = s.Name });
                }
            }
        }

        void RetrieveStationData(object sender, SelectionChangedEventArgs args)
        {
            StationData.Text = "";
            ListViewItem selected = ((sender as ListView).SelectedItem as ListViewItem);
            if (selected != null)
            {
                VelibLibrary.Station station = stationInfo[selected.Content.ToString()];
                if (station == null)
                {
                    StationData.Text = "This station does not exist";
                }
                else
                {
                    StationData.Text = Present(station);
                    loadImage(station.Latitude, station.Longitude);
                }
            }
        }
    }
}
