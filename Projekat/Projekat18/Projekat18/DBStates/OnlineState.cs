using Projekat18.Model.Enums;
using Projekat18.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat18.DBStates
{
    public class OnlineState : IDatabaseState
    {
        public string Name => "Online";
        public string Color => "#4CAF50"; // zelena
        public void Handle(Database db)
        {
            // ponašanje za online
        }
    }
}
