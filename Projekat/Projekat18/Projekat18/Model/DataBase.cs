using Projekat18.Model.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace Projekat18.Model
{
    public class Database : INotifyPropertyChanged
    {
        private string _provider;
        public string Provider
        {
            get => _provider;
            set { _provider = value; OnPropertyChanged(nameof(Provider)); }
        }

        private DatabaseType _type;
        public DatabaseType Type
        {
            get => _type;
            set { _type = value; OnPropertyChanged(nameof(Type)); }
        }

        private string _queryLanguage;
        public string QueryLanguage
        {
            get => _queryLanguage;
            set { _queryLanguage = value; OnPropertyChanged(nameof(QueryLanguage)); }
        }

        private ObservableCollection<Table> _tables;
        public ObservableCollection<Table> Tables
        {
            get => _tables;
            set { _tables = value; OnPropertyChanged(nameof(Tables)); }
        }

        private Administrator _admin;
        public Administrator Admin
        {
            get => _admin;
            set { _admin = value; OnPropertyChanged(nameof(Admin)); }
        }

        private IDatabaseState _state;
        public IDatabaseState State
        {
            get => _state;
            set { _state = value; OnPropertyChanged(nameof(State)); }
        }

        public Database()
        {
            Tables = new ObservableCollection<Table>();
        }

        public Database(string provider, DatabaseType type, string queryLanguage, ObservableCollection<Table> tables, Administrator admin, IDatabaseState state)
        {
            Provider = provider;
            Type = type;
            QueryLanguage = queryLanguage;
            Tables = tables ?? new ObservableCollection<Table>();
            Admin = admin;
            State = state;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
