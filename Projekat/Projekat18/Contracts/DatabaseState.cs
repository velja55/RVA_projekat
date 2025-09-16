using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    [DataContract]
    public enum DatabaseState
    {
        [EnumMember]
        Online,
        [EnumMember]
        Recovering,
        [EnumMember]
        Restoring,
        [EnumMember]
        Offline
    }
}
