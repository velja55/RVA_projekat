using Projekat18.DBStates;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using Projekat18.Model;
using Table = Projekat18.Model.Table;

namespace Projekat18.MapperHelper
{
    public class TableMapper
    {
        public static ObservableCollection<Table> FromContract(List<Contracts.Table> contractDb)
        {
            ObservableCollection<Table> tables = new ObservableCollection<Table>();

            foreach (Contracts.Table contract in contractDb)
            {
                tables.Add(new Table { Name = contract.Name, ColumnHeaders = contract.ColumnHeaders });
            }

            return tables;
        }

        public static List<Contracts.Table> FromModel(ObservableCollection<Table> modelDb)
        {
            List<Contracts.Table> tables = new List<Contracts.Table>();

            foreach (Table contract in modelDb)
            {
                tables.Add(new Contracts.Table { Name = contract.Name, ColumnHeaders = contract.ColumnHeaders });
            }

            return tables;
        }
    }
}
