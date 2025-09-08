using Projekat18.Helpers;
using Projekat18.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat18.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
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

        public MainWindowViewModel()
        {
            CurrentView = new DatabaseView(); // Prvi prikaz

            ShowDatabaseViewCommand = new MyICommand(() => CurrentView = new DatabaseView());
          
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}
