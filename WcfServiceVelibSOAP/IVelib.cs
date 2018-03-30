using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Threading.Tasks;

namespace VelibLibrary
{
    [ServiceContract]
    public interface IVelib
    {
        [OperationContract]
        Task<IList<string>> GetCitiesAsync();

        [OperationContract]
        Task<IList<Station>> GetStationsAsync(string city);

    }

    [DataContract]
    public class Station
    {
        [DataMember]
        public int Number { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Address { get; set; }

        [DataMember]
        public string City { get; set; }

        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public int TotalStands { get; set; }

        [DataMember]
        public int AvailableStands { get; set; }

        [DataMember]
        public double Latitude { get; set; }

        [DataMember]
        public double Longitude { get; set; }
    }
}
