using ECTunesDB.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECTunes.Database.ConfigLibrary {
    public interface IDbConnectorRemote: ICustomerService, ICarService, IParamService, ISignalService {

        DateTime AddToDatabase(String customerName, String carName, List<ParamDC> paramList, List<SignalDC> signalList);

        bool IsConnected();

    }
}
