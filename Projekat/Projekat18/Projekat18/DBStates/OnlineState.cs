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
    public class OnlineState : IDatabaseState
    {
        public string Name => "Online";
        public string Color => "#4CAF50";
        public void Handle(Database db)
        {
            MessageBox.Show($"Database{db.Provider}-{db.QueryLanguage} is online. All operations can be performed.", "Online State", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
