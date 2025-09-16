using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WcfDatabaseServer.Services
{
    public interface IDataStorage
    {
        List<DataBase> LoadDatabases();
        void SaveDatabases(List<DataBase> dbs);
        List<LegacyDatabase> LoadLegacyDatabases();
        void SaveLegacyDatabases(List<LegacyDatabase> legacies);
    }
}
