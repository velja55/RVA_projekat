using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    [DataContract]
    public enum DatabaseType
    {
        [EnumMember]
        RELATIONAL,
        [EnumMember]
        NOSQL,
        [EnumMember]
        VECTOR
    }
}
