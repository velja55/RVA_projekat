using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    [DataContract]
    public class DataBase
    {
        [DataMember]
        public string Provider { get; set; }
        [DataMember]
        public DatabaseType Type { get; set; }
        [DataMember]
        public string QueryLanguage { get; set; }
        [DataMember]
        public List<Table> Tables { get; set; }
        [DataMember]
        public Administrator Admin { get; set; }
        [DataMember]
        public DatabaseState State { get; set; }
    }
}