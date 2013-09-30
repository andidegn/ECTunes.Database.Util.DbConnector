using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ECTunesDB.model {

    [ServiceContract]
    public interface IParamService {

        [OperationContract]
        List<ParamDC> GetParamList(int carId);
    }

    [Serializable]
    [DataContract]
    public class ParamDC {
        [DataMember]
        public int carId { get; set; }
        [DataMember]
        public string path { get; set; }
        [DataMember]
        public string subPath { get; set; }
        [DataMember]
        public Nullable<int> value { get; set; }
    }
}
