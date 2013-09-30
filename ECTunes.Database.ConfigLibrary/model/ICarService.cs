using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ECTunesDB.model {

    [ServiceContract]
    public interface ICarService {

        [OperationContract]
        CarDC GetCar(int customerId, String name, DateTime version);
        [OperationContract]
        List<CarDC> GetCars();
        [OperationContract]
        List<CarDC> GetCars(int customerId);
        [OperationContract]
        List<CarDC> GetCars(int customerId, String carName);
        [OperationContract]
        int GetCarId(int customerId, String carName, DateTime version);
        [OperationContract]
        DateTime GetLatestVersion(int customerId, String carName);
        [OperationContract]
        bool DeleteCar(String customerName, String carName, DateTime version);

    }

    [Serializable]
    [DataContract]
    public class CarDC {
        [DataMember]
        public int customerId { get; set; }
        [DataMember]
        public int carId { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public System.DateTime version { get; set; }
    }
}
