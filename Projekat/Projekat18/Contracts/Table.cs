using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    [DataContract]
    public class Table
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public List<string> ColumnHeaders { get; set; }
    }
}