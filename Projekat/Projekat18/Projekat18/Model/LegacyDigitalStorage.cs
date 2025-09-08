using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat18.Model
{
    public abstract class LegacyDigitalStorage
    {
        public string StorageAdmin { get; set; }
        public int NumberOfTables { get; set; }

        // Prazan konstruktor
        public LegacyDigitalStorage() { }

        // Puni konstruktor
        public LegacyDigitalStorage(string storageAdmin, int numberOfTables)
        {
            StorageAdmin = storageAdmin;
            NumberOfTables = numberOfTables;
        }
    }
}
