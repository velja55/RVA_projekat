using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public abstract class LegacyDigitalStorage
    {
        public string StorageAdmin { get; set; }
        public int NumberOfTables { get; set; }
    }
}