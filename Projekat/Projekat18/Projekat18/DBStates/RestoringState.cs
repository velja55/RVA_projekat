using Projekat18.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Projekat18.DBStates
{
    public class RestoringState : IDatabaseState
    {
        public string Name => "Restoring";
        public string Color => "#2196F3";
        public void Handle(Database db)
        {
            MessageBox.Show($"Database{db.Provider}-{db.QueryLanguage} is restoring. No operations can be performed.", "Restoring State", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}
