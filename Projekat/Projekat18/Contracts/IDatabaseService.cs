using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    [ServiceContract]
    public interface IDatabaseService
    {
        [OperationContract]
        List<DataBase> GetDatabases();

        [OperationContract]
        void AddDatabase(DataBase db);

        [OperationContract]
        void UpdateDatabase(DataBase db, string oldProvider);

        [OperationContract]
        void DeleteDatabase(string provider);

        [OperationContract]
        List<Table> GetTables(string provider);

        [OperationContract]
        void AddTable(string provider, Table tbl);

        [OperationContract]
        List<LegacyDatabase> GetLegacyDatabases();

        [OperationContract]
        void AddLegacyDatabase(LegacyDatabase db);

        [OperationContract]
        void SetStorageFormat(string format);

        [OperationContract]
        Dictionary<DatabaseState, int> GetStateCounts();
    }
}
