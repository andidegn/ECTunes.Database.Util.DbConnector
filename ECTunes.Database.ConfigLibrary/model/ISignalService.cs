using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ECTunesDB.model {

    [ServiceContract]
    public interface ISignalService {

        [OperationContract]
        List<SignalDC> GetSignalList(int carId);
    }

    [Serializable]
    [DataContract]
    public class SignalDC {
        [DataMember]
        public int carId { get; set; }
        [DataMember]
        public int signalId { get; set; }
        [DataMember]
        public string type { get; set; }
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public int startbit { get; set; }
        [DataMember]
        public int size { get; set; }
        [DataMember]
        public string format { get; set; }
        [DataMember]
        public string signed { get; set; }
        [DataMember]
        public Nullable<double> factor { get; set; }
        [DataMember]
        public Nullable<int> offset { get; set; }
        [DataMember]
        public Nullable<double> min { get; set; }
        [DataMember]
        public Nullable<int> max { get; set; }
        [DataMember]
        public Nullable<int> match { get; set; }
    }
}
