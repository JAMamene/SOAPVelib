using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace VelibLibrary
{
    [ServiceContract(SessionMode = SessionMode.Required,
          CallbackContract = typeof(IVelibClientContract))]
    public interface IVelibEventContract
    {
        [OperationContract(IsOneWay = false, IsInitiating = true)]
        void Subscribe(string station, string city, int time);
    }

    public interface IVelibClientContract
    {
        [OperationContract(IsOneWay = true)]
        void VelibChanged(string station, string city, int Before, int After, int time);
    }

}
