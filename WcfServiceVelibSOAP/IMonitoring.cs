using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace VelibLibrary
{
    [ServiceContract]
    public interface IMonitoring
    {
        [OperationContract]
        RequestInfo GetWSRequestsTimeSpan(DateTime beg, DateTime end);

        [OperationContract]
        RequestInfo GetWSRequests();

        [OperationContract]
        Metrics GetMetrics();

    }

    [DataContract]
    public class RequestInfo
    {
        [DataMember]
        public int Cached { get; set; }

        [DataMember]
        public int Normal { get; set; }
    }

    [DataContract]
    public class Metrics
    {
        [DataMember]
        public long Min { get; set; }

        [DataMember]
        public long Max { get; set; }

        [DataMember]
        public long Avg { get; set; }
    }
}
