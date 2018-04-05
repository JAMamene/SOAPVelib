using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleClienSubscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            VelibEventSink sink = new VelibEventSink();
            InstanceContext iCntxt = new InstanceContext(sink);

            ServiceReference.VelibEventContractClient client = new ServiceReference.VelibEventContractClient(iCntxt);
            Console.WriteLine("Enter a City and a station number where you want to subscribe (Example : Rouen, 7)");
            Console.WriteLine("City :");
            String city = Console.ReadLine();
            Console.WriteLine("Station Number: ");
            String station = Console.ReadLine();
            Console.WriteLine("Set a refresh rate in minutes : ");
            int refreshRate = Int32.Parse(Console.ReadLine());
            client.Subscribe(station, city, refreshRate);
            // Wait for updates
            Console.ReadKey();
        }
    }
}
