using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat18.Model
{
    public class LegacyDatabase:LegacyDigitalStorage
    {
        public string DbSystemName { get; set; }
        public string ApproachToStoring { get; set; }
        public string InstructionSyntax { get; set; }

        // Prazan konstruktor
        public LegacyDatabase() { }

        // Puni konstruktor
        public LegacyDatabase(string dbSystemName, string approachToStoring, string instructionSyntax)
        {
            DbSystemName = dbSystemName;
            ApproachToStoring = approachToStoring;
            InstructionSyntax = instructionSyntax;
        }
    }
}
