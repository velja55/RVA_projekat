using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat18.MapperHelper
{
    public class LegacyDatabaseMapper
    {
        public static LegacyDatabase FromModel(Projekat18.Model.LegacyDatabase modelDb)
        {
            return new LegacyDatabase
            {
                DBSystemName = modelDb.DbSystemName,
                InstructionSyntax = modelDb.InstructionSyntax,
                NumberOfTables = modelDb.NumberOfTables,
                StorageAdmin = modelDb.StorageAdmin,
                ApproachToStoring = "Line by line"
            };
        }
    }
}
