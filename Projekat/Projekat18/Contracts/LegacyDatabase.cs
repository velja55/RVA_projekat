using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    [DataContract]
    public class LegacyDatabase
    {
        [DataMember]
        public string DBSystemName { get; set; }
        [DataMember]
        public string ApproachToStoring { get; set; }
        [DataMember]
        public string InstructionSyntax { get; set; }
    }
}