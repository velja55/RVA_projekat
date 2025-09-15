using Projekat18.DBStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat18.Helpers
{
    public static class DatabaseStateFactory
    {
        public static IDatabaseState CreateState(string stateName)
        {
            switch (stateName?.Trim().ToLower())
            {
                case "online": return new OnlineState();
                case "recovering": return new RecoveringState();
                case "restoring": return new RestoringState();
                case "offline": return new OfflineState();
                default: return new OnlineState();
            }
        }
    }
}
