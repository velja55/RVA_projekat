using Projekat18.DBStates;
using Projekat18.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat18.Helpers
{
    public static class DatabaseStateHelper
    {
        public static List<IDatabaseState> GetStates => new List<IDatabaseState>
        {
            new OnlineState(),
            new RecoveringState(),
            new RestoringState(),
            new OfflineState()
        };
    }
}
