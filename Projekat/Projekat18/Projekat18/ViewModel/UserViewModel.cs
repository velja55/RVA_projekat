using Projekat18.Helpers;
using Projekat18.Model;
using Projekat18.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat18.ViewModel
{
    public  class UserViewModel : BaseViewModel
    {
        private object _currentView;
        public object CurrentView
        {
            get => _currentView;
            set { _currentView = value; OnPropertyChanged(nameof(CurrentView)); }
        }

        public MyICommand ShowDatabaseViewCommand { get; }
        public MyICommand ShowTableViewCommand { get; }
        public MyICommand ShowLegacyViewCommand { get; }

        public UserViewModel(Administrator administrator)
        {
            CurrentView = new DatabaseView(administrator);

            ShowDatabaseViewCommand = new MyICommand(() => CurrentView = new DatabaseView(administrator));
        }
    }
}
