using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    [DataContract]
    public class Administrator
    {
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string Certificate { get; set; }
        [DataMember]
        public string Permissions { get; set; }
        [DataMember]
        public string Password { get; set; }
    }
}
