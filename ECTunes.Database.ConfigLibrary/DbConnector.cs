using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using ECTunesDB;
using ECTunesDB.model;
using ECTunes.Database.ConfigLibrary;
using System.Text;


namespace ECTunes.Database.Util {
    [Serializable]
    public class DbConnector : MarshalByRefObject, IDbConnectorRemote, IDbConnectorLocal {

        public const String SERVER_PATH = "ECTunes.Server.Config";
        /// <summary>
        /// 0: '127.0.0.1'<br />
        /// 1: '192.168.0.101'<br />
        /// 2: '188.180.104.131'<br />
        /// 3: 'andidegn.dk'
        /// 
        /// </summary>
        public static readonly String[] IP = new String[] { "127.0.0.1", "192.168.0.101", "188.180.104.131", "andidegn.dk" };
        public const int PORT = 9090;

        public const String USERNAME = "config";

        public DbConnector() { }

        public ECTunesDatabaseConfig GetDb() {
            return new ECTunesDatabaseConfig();
        }

        public bool IsConnected() {
            return true;
        }

        #region customer
        public CustomerDC GetCustomer(String name) {
            using (ECTunesDatabaseConfig DB = new ECTunesDatabaseConfig()) {
                var customerQuery = (from r in DB.Customer
                                     where r.name == name
                                     select r).SingleOrDefault();
                if (customerQuery != null)
                    return Translate(customerQuery as Customer);
                return null;
            }
        }

        public List<CustomerDC> GetCustomers() {
            using (ECTunesDatabaseConfig DB = new ECTunesDatabaseConfig()) {
                var customerQuery = from r in DB.Customer
                                    orderby r.name
                                    select r;
                return TranslateList(customerQuery.ToList<Customer>());
            }
        }

        public int GetCustomerId(String name) {
            using (ECTunesDatabaseConfig DB = new ECTunesDatabaseConfig()) {
                var customerIdQuery = (from r in DB.Customer
                                       where r.name == name
                                       select r).SingleOrDefault();
                if (customerIdQuery != null)
                    return customerIdQuery.customerId;
                return -1;
            }
        }

        public bool DeleteCustomer(String name) {
            using (ECTunesDatabaseConfig DB = new ECTunesDatabaseConfig()) {
                var customerQuery = (from r in DB.Customer
                                     where r.name == name
                                     select r).SingleOrDefault();
                if (customerQuery != null) {
                    DB.Customer.Remove(customerQuery);
                    return DB.SaveChanges() > 0 ? true : false;
                }
                return false;
            }
        }

        private CustomerDC Translate(Customer customer) {
            CustomerDC c = new CustomerDC();
            c.customerId = customer.customerId;
            c.name = customer.name;
            return c;
        }

        private List<CustomerDC> TranslateList(List<Customer> customerList) {
            if (customerList == null)
                return null;
            List<CustomerDC> cl = new List<CustomerDC>();
            foreach (var customer in customerList)
                cl.Add(Translate(customer as Customer));
            return cl;
        }
        #endregion

        #region car
        public CarDC GetCar(int customerId, String carName, DateTime version) {
            using (ECTunesDatabaseConfig DB = new ECTunesDatabaseConfig()) {
                var CarQuery = (from r in DB.Car
                                where r.customerId == customerId &&
                                r.name == carName &&
                                r.version == version
                                select r).SingleOrDefault();
                if (CarQuery != null)
                    return Translate(CarQuery as Car);
                return null;
            }
        }

        public List<CarDC> GetCars() {
            using (ECTunesDatabaseConfig DB = new ECTunesDatabaseConfig()) {
                var carQuery = from r in DB.Car
                               select r;
                return TranslateList(carQuery.ToList<Car>());
            }
        }

        public List<CarDC> GetCars(int customerId) {
            using (ECTunesDatabaseConfig DB = new ECTunesDatabaseConfig()) {
                var carQuery = from r in DB.Car
                               where r.customerId == customerId
                               orderby r.name
                               ascending, r.version
                               descending
                               select r;
                return TranslateList(carQuery.ToList<Car>());
            }
        }

        public List<CarDC> GetCars(int customerId, String carName) {
            using (ECTunesDatabaseConfig DB = new ECTunesDatabaseConfig()) {
                var carQuery = from r in DB.Car
                               where r.customerId == customerId && 
                               r.name == carName
                               select r;
                return TranslateList(carQuery.ToList<Car>());
            }
        }

        public int GetCarId(int customerId, String carName, DateTime version) {
            try {
                using (ECTunesDatabaseConfig DB = new ECTunesDatabaseConfig()) {
                    var carQuery = (from r in DB.Car
                                    where r.name == carName && 
                                    r.customerId == customerId && 
                                    r.version == version
                                    select r).SingleOrDefault();
                    if (carQuery != null)
                        return carQuery.carId;
                    return -1;
                }

            }
            catch (Exception) {
                return -1;
            }
        }

        public DateTime GetLatestVersion(int customerId, String carName) {
            using (ECTunesDatabaseConfig DB = new ECTunesDatabaseConfig()) {
                var carVersionQuery = (from r in DB.Car
                                       where r.customerId == customerId && r.name == carName
                                       select r.version).Max();
                if (carVersionQuery != null)
                    return carVersionQuery;
                return DateTime.MinValue;
            }
        }

        public bool DeleteCar(String customerName, String carName, DateTime version) {
            using (ECTunesDatabaseConfig DB = new ECTunesDatabaseConfig()) {
                int _customerId = GetCustomerId(customerName);
                var carQuery = (from r in DB.Car
                                where r.customerId == _customerId &&
                                r.name == carName &&
                                r.version == version
                                select r).SingleOrDefault();
                if (carQuery != null) {
                    DB.Car.Remove(carQuery);
                    return DB.SaveChanges() > 0 ? true : false;
                }
                return false;
            }
        }

        private CarDC Translate(Car car) {
            CarDC c = new CarDC();
            c.carId = car.carId;
            c.customerId = car.customerId;
            c.name = car.name;
            c.version = car.version;
            return c;
        }

        private List<CarDC> TranslateList(List<Car> carList) {
            if (carList == null)
                return null;
            List<CarDC> cl = new List<CarDC>();
            foreach (var car in carList)
                cl.Add(Translate(car as Car));
            return cl;
        }
        #endregion

        #region param
        public List<ParamDC> GetParamList(int carId) {
            using (ECTunesDatabaseConfig DB = new ECTunesDatabaseConfig()) {
                var paramQuery = from r in DB.Param
                                 where r.carId == carId
                                 orderby r.subPath
                                 select r;
                return TranslateList(paramQuery.ToList<Param>());
            }
        }

        public List<ParamDC> GetParamList() {
            using (ECTunesDatabaseConfig DB = new ECTunesDatabaseConfig()) {
                var paramQuery = from r in DB.Param
                                 orderby r.carId
                                 select r;
                return TranslateList(paramQuery.ToList<Param>());
            }
        }

        private List<ParamDC> TranslateList(List<Param> paramList) {
            if (paramList == null)
                return null;
            List<ParamDC> pl = new List<ParamDC>();
            foreach (var param in paramList)
                pl.Add(Translate(param));
            return pl;
        }

        private ParamDC Translate(Param param) {
            ParamDC p = new ParamDC();
            p.carId = param.carId;
            p.subPath = param.subPath;
            p.path = param.path;
            p.value = param.value;
            return p;
        }

        private Param Translate(ParamDC param) {
            Param p = new Param();
            p.carId = param.carId;
            p.subPath = param.subPath;
            p.path = param.path;
            p.value = param.value;
            return p;
        }
        #endregion

        #region signal
        public List<SignalDC> GetSignalList(int carId) {
            using (ECTunesDatabaseConfig DB = new ECTunesDatabaseConfig()) {
                var signalQuery = from r in DB.Signal
                                  where r.carId == carId
                                  orderby r.type
                                  select r;
                return TranslateList(signalQuery.ToList<Signal>());
            }
        }

        public List<SignalDC> GetSignalList() {
            using (ECTunesDatabaseConfig DB = new ECTunesDatabaseConfig()) {
                var signalQuery = from r in DB.Signal
                                  orderby r.carId
                                  select r;
                return TranslateList(signalQuery.ToList<Signal>());
            }
        }

        private List<SignalDC> TranslateList(List<Signal> signalList) {
            if (signalList == null)
                return null;
            List<SignalDC> sl = new List<SignalDC>();
            foreach (var signal in signalList)
                sl.Add(Translate(signal as Signal));
            return sl;
        }

        private SignalDC Translate(Signal signal) {
            SignalDC s = new SignalDC();
            s.carId = signal.carId;
            s.signalId = signal.signalId;
            s.type = signal.type;
            s.id = signal.id;
            s.startbit = signal.startbit;
            s.size = signal.size;
            s.format = signal.format;
            s.signed = signal.signed;
            s.factor = signal.factor;
            s.offset = signal.offset;
            s.min = signal.min;
            s.max = signal.max;
            s.match = signal.match;
            return s;
        }

        private Signal Translate(SignalDC signal) {
            Signal s = new Signal();
            s.carId = signal.carId;
            s.signalId = signal.signalId;
            s.type = signal.type;
            s.id = signal.id;
            s.startbit = signal.startbit;
            s.size = signal.size;
            s.format = signal.format;
            s.signed = signal.signed;
            s.factor = signal.factor;
            s.offset = signal.offset;
            s.min = signal.min;
            s.max = signal.max;
            s.match = signal.match;
            return s;
        }
        #endregion

        private int GetMax(int a, int b) {
            return a >= b ? a : b;
        }

        ///// <summary>
        ///// Adds a single customer to the database
        ///// </summary>
        ///// <param name="customerNode"></param>
        ///// <param name="db"></param>
        //public void AddSingleToDatabase(XmlNode customerNode, String carToStore, int i) {
        //    List<String> l = new List<string>();
        //    l.Add(carToStore);
        //    //AddRangeToDatabase(customerNode, l);
        //}

        /// <summary>
        /// Adds a customer to the database
        /// </summary>
        /// <param name="customerNode"></param>
        /// <param name="db"></param>
        public DateTime AddToDatabase(String customerName, String carName, List<ParamDC> paramList, List<SignalDC> signalList) {
            using (ECTunesDatabaseConfig DB = new ECTunesDatabaseConfig()) {
                if (customerName.Length > 1) {
                    int _customerId = 0;
                    // Checking if customer already exists
                    var q1 = from r in DB.Customer // update so that it uses the function
                             where r.name == customerName
                             select new { r.customerId };
                    if (q1.Count() == 0) {
                        Customer customer = new Customer();
                        customer.name = customerName;
                        DB.Customer.Add(customer);
                        DB.SaveChanges();
                        // Getting the customerId from added customer
                        _customerId = (from r in DB.Customer
                                       where r.name == customerName
                                       select new { r.customerId }).SingleOrDefault().customerId;
                    }
                    else {
                        // Getting the customerId from existing customer
                        _customerId = q1.Single().customerId;
                    }

                    // Car
                    Car car = new Car();
                    car.customerId = _customerId;
                    car.name = carName;
                    car.version = DateTime.Now;
                    DB.Car.Add(car);
                    DB.SaveChanges();

                    // Getting most recent added carId
                    var q3 = from r in DB.Car where r.name == car.name select new { r.carId }; // Change to .Max();
                    int _carId = 0;
                    foreach (var item in q3) { _carId = GetMax(_carId, item.carId); }

                    foreach (ParamDC p in paramList) { // Parameter
                        p.carId = _carId;
                        DB.Param.Add(Translate(p));
                    }

                    foreach (SignalDC s in signalList) { // Signal
                        s.carId = _carId;
                        DB.Signal.Add(Translate(s));
                    }
                    DB.SaveChanges();
                    return GetLatestVersion(_customerId, carName);
                }
                return DateTime.MinValue;
            }
        }

        private void PrintException(String function, Exception msg) {
            StringBuilder sb = new StringBuilder();
            sb.Append("\nError in: '");
            sb.Append(function);
            sb.Append("'\nException:\n");
            sb.Append(msg.Data);
            sb.Append("\nMsg:\n");
            sb.Append(msg.Message);
            sb.Append("\nStackTrace:\n");
            sb.Append(msg.StackTrace);
            sb.Append("\nInnerException:\n");
            sb.Append(msg.InnerException);
            Console.WriteLine(sb.ToString());
        }
    }
}
