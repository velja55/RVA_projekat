using Projekat18.Model.Enums;
using Projekat18.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat18.DBStates
{
    public class OfflineState : IDatabaseState
    {
        public string Name => "Offline";
        public string Color => "#9E9E9E"; // siva
        public void Handle(Database db)
        {
            // ponašanje za offline
        }
    }
}
