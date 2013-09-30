using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ECTunesDB.model {

    [ServiceContract]
    public interface ICustomerService {

        [OperationContract]
        CustomerDC GetCustomer(String name);
        [OperationContract]
        List<CustomerDC> GetCustomers();
        [OperationContract]
        int GetCustomerId(String name);
        [OperationContract]
        bool DeleteCustomer(String name);

    }

    [Serializable]
    [DataContract]
    public class CustomerDC {

        [DataMember]
        public int customerId { get; set; }
        [DataMember]
        public string name { get; set; }

    }
}
