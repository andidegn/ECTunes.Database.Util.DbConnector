using ECTunesDB.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECTunes.Database.ConfigLibrary {
    public interface IDbConnectorLocal: IDbConnectorRemote {

        ECTunesDatabaseConfig GetDb();

        List<ParamDC> GetParamList();

        List<SignalDC> GetSignalList();

    }
}
