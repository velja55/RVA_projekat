using Projekat18.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat18.DBStates
{
    public class RestoringState : IDatabaseState
    {
        public string Name => "Restoring";
        public string Color => "#2196F3"; // plava
        public void Handle(Database db)
        {
            // ponašanje za restoring
        }
    }
}
