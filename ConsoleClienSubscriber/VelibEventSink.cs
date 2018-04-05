using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleClienSubscriber
{
    class VelibEventSink : ServiceReference.IVelibEventContractCallback
    {

        public void VelibChanged(string station, string city, int before, int after, int time)
        {
            Console.WriteLine("Station n°= " + station + " in " + city + "\n Number of velibs available before : " + before + " and now : " + after + "\n Next update in " + time + " minutes");
        }
    }
}
