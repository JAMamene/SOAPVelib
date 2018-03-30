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

namespace GUIClienMonitoring
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private VelibLibrary.MonitoringClient client;

        private int minutes = 0;

        public MainWindow()
        {
            InitializeComponent();
            minutesLabel.Text = minutes.ToString();
            client = new VelibLibrary.MonitoringClient();
        }

        private void Refresh(object sender, RoutedEventArgs e)
        {
            if (minutes == 0)
            {
                sinceTime.Content = "";
                VelibLibrary.RequestInfo info = client.GetWSRequests();
                WSRequest.Content = info.Normal;
                CachedRequest.Content = info.Cached;
            }
            else
            {
                sinceTime.Content = "since " + minutes + " minutes";
                VelibLibrary.RequestInfo info = client.GetWSRequestsTimeSpan(DateTime.Now.Add(new TimeSpan(0, -minutes, 0)), DateTime.Now);
                WSRequest.Content = info.Normal;
                CachedRequest.Content = info.Cached;
            }
            VelibLibrary.Metrics metrics = client.GetMetrics();
            avgDelay.Content = metrics.Avg;
            minDelay.Content = metrics.Min;
            maxDelay.Content = metrics.Max;
        }

        public int Minutes
        {
            get { return minutes; }
            set
            {
                minutes = value;
                minutesLabel.Text = value.ToString();
            }
        }

        private void cmdUp_Click(object sender, RoutedEventArgs e)
        {
            Minutes++;
        }

        private void cmdDown_Click(object sender, RoutedEventArgs e)
        {
            if (minutes > 0)
            {
                Minutes--;
            }
        }

        private void minutesLabel_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (minutesLabel == null)
            {
                return;
            }

            if (!int.TryParse(minutesLabel.Text, out minutes))
                minutesLabel.Text = minutes.ToString();
        }
    }
}
