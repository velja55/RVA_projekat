using Projekat18.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Projekat18.DBStates
{
    public class RecoveringState : IDatabaseState
    {
        public string Name => "Recovering";
        public string Color => "#FF9800";
        public void Handle(Database db)
        {
            MessageBox.Show($"Database{db.Provider}-{db.QueryLanguage} is recovering. Limited operations can be performed.", "Recovering State", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
