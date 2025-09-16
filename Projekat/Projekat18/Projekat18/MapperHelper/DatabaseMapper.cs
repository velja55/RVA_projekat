using Contracts;
using Projekat18.DBStates;
using Projekat18.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Table = Projekat18.Model.Table;

namespace Projekat18.MapperHelper
{
    public class DatabaseMapper
    {
        public static Database FromContract(DataBase contractDb)
        {
            IDatabaseState state;
            switch (contractDb.State)
            {
                case DatabaseState.Online:
                    state = new OnlineState();
                    break;
                case DatabaseState.Recovering:
                    state = new RecoveringState();
                    break;
                case DatabaseState.Restoring:
                    state = new RestoringState();
                    break;
                case DatabaseState.Offline:
                    state = new OfflineState();
                    break;
                default:
                    state = new OfflineState();
                    break;
            }

            return new Database
            {
                Provider = contractDb.Provider,
                Type = (Model.Enums.DatabaseType)contractDb.Type,
                QueryLanguage = contractDb.QueryLanguage,
                Tables = new ObservableCollection<Table>(TableMapper.FromContract(contractDb.Tables)),
                Admin = AdministratorMapper.FromContract(contractDb.Admin),
                State = state
            };
        }

        public static DataBase FromModel(Database modelDb)
        {
            DatabaseState state;

            if (modelDb.State is OnlineState)
                state = DatabaseState.Online;
            else if (modelDb.State is RecoveringState)
                state = DatabaseState.Recovering;
            else if (modelDb.State is RestoringState)
                state = DatabaseState.Restoring;
            else if (modelDb.State is OfflineState)
                state = DatabaseState.Offline;
            else
                state = DatabaseState.Offline;

            return new DataBase
            {
                Provider = modelDb.Provider,
                Type = (Contracts.DatabaseType)modelDb.Type,
                QueryLanguage = modelDb.QueryLanguage,
                Tables = new List<Contracts.Table>(TableMapper.FromModel(modelDb.Tables)),
                Admin = AdministratorMapper.FromModel(modelDb.Admin),
                State = state
            };
        }
    }
}
