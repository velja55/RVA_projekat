using Projekat18.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat18.DBStates
{
    public class RecoveringState : IDatabaseState
    {
        public string Name => "Recovering";
        public string Color => "#FF9800"; // narandžasta
        public void Handle(Database db)
        {
            // ponašanje za recovering
        }
    }
}
