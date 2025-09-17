using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    [DataContract]
    public abstract class LegacyDigitalStorage
    {
        [DataMember]
        public string StorageAdmin { get; set; }
        [DataMember]
        public int NumberOfTables { get; set; }
    }
}