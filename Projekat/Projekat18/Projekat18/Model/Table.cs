using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat18.Model
{
    public class Table
    {
        public string Name { get; set; }
        public List<string> ColumnHeaders { get; set; }

        public string ColumnsPreview => string.Join(", ", ColumnHeaders ?? new List<string>());

        public Table()
        {
            ColumnHeaders = new List<string>();
        }

        public Table(string name, List<string> columnHeaders)
        {
            Name = name;
            ColumnHeaders = columnHeaders ?? new List<string>();
        }

        public override bool Equals(object obj)
        {
            if (obj is Table other)
                return this.Name == other.Name;
            return false;
        }

        public override int GetHashCode() => Name.GetHashCode();
    }
}
