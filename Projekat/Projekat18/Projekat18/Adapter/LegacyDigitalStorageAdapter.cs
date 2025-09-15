using Projekat18.Model.Enums;
using Projekat18.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.AxHost;
using Projekat18.Helpers;

namespace Projekat18.Adapter
{
    public class LegacyDigitalStorageAdapter : Database
    {
        public LegacyDigitalStorageAdapter(LegacyDatabase legacyDb,string state)
        {
            // Popuni property-je Database klase iz legacy objekata
            Provider = legacyDb.DbSystemName;
            Type = DatabaseType.RELATIONAL; // ili omogući mapiranje iz legacy
            QueryLanguage = legacyDb.InstructionSyntax;
            Tables = new ObservableCollection<Table>();
            for (int i = 0; i < legacyDb.NumberOfTables; i++)
            {
                Tables.Add(new Table { Name = $"LegacyTable{i + 1}", ColumnHeaders = new List<string>() });
            }
            Admin = new Administrator { UserName = legacyDb.StorageAdmin };
            State =  DatabaseStateFactory.CreateState(state); // ili neki adapter za stanje, po potrebi
        }
    }
}
