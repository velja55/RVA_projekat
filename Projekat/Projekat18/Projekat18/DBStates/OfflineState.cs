using Projekat18.Model.Enums;
using Projekat18.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel.Channels;
using System.Windows;

namespace Projekat18.DBStates
{
    public class OfflineState : IDatabaseState
    {
        public string Name => "Offline";
        public string Color => "#9E9E9E"; // siva
        public void Handle(Database db)
        {
            MessageBox.Show($"Database{db.Provider}-{db.QueryLanguage} is offline. No operations can be performed.", "Offline State", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}
