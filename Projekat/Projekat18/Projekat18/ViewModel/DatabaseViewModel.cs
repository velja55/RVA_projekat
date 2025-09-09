using Projekat18.Helpers;
using Projekat18.Model.Enums;
using Projekat18.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Projekat18.ViewModel
{
    public class DatabaseViewModel : BaseViewModel
    {
        #region Fields and Properties
        public ObservableCollection<Database> Databases { get; set; }
        public ObservableCollection<Database> FilteredDatabases { get; set; }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set { _searchText = value; OnPropertyChanged(nameof(SearchText)); }
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; OnPropertyChanged(nameof(ErrorMessage)); }
        }

        private bool _isEditVisible;
        public bool IsEditVisible
        {
            get => _isEditVisible;
            set { _isEditVisible = value; OnPropertyChanged(nameof(IsEditVisible)); }
        }

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

        private DatabaseState _state;
        public DatabaseState State
        {
            get => _state;
            set { _state = value; OnPropertyChanged(nameof(State)); }
        }

        private Database _selectedDatabase;
        public Database SelectedDatabase
        {
            get => _selectedDatabase;
            set { _selectedDatabase = value; OnPropertyChanged(nameof(SelectedDatabase)); LoadEditFields(); }
        }
        #endregion


        #region Commands
        public MyICommand SearchCommand { get; }
        public MyICommand ResetSearchCommand { get; }

        public MyICommand CancelEditCommand { get; }
        public MyICommand<Database> EditRowCommand { get; }
        public MyICommand<Database> RemoveRowCommand { get; }
        public MyICommand AddDatabaseCommand { get; }
        public MyICommand EditDatabaseCommand { get; }
        public MyICommand RemoveDatabaseCommand { get; }
        #endregion  
        public DatabaseViewModel(Administrator u)
        {
            Databases = new ObservableCollection<Database>
            {
                new Database("SQL Server", DatabaseType.RELATIONAL, "T-SQL", null, new Administrator("Stefan", "Admin", "Add/Edit/Delete", "Stefan"), DatabaseState.Online),
                new Database("MongoDB", DatabaseType.NOSQL, "MongoQL", null, new Administrator("Veljko", "Admin", "Add/Edit/Delete", "Veljko"), DatabaseState.Offline)
               
            };
            FilteredDatabases = new ObservableCollection<Database>(Databases);
            ErrorMessage= "";
            SearchCommand = new MyICommand(SearchDatabases);
            ResetSearchCommand = new MyICommand(ResetSearch);
            AddDatabaseCommand = new MyICommand(AddDatabase);
            EditDatabaseCommand = new MyICommand(EditDatabase);
            RemoveDatabaseCommand = new MyICommand(RemoveDatabase, () => SelectedDatabase != null);
            EditRowCommand = new MyICommand<Database>(EditRow);
            RemoveRowCommand = new MyICommand<Database>(RemoveRow);
            CancelEditCommand = new MyICommand(() => { IsEditVisible = false; ClearFields(); SelectedDatabase = null; });
        }

        #region Methods

        private void SearchDatabases()
        {
            FilteredDatabases.Clear();
            var query = SearchText?.ToLower() ?? "";
            foreach (var db in Databases)
            {
                if (string.IsNullOrWhiteSpace(query) || IsMatch(db, query))
                    FilteredDatabases.Add(db);
            }
        }

        private bool IsMatch(Database db, string query)
        {
            return (db.Provider?.ToLower().Contains(query) ?? false)
                || (db.Type.ToString().ToLower().Contains(query))
                || (db.QueryLanguage?.ToLower().Contains(query) ?? false)
                || (db.State.ToString().ToLower().Contains(query));
                //|| (db.Administrator?.Name?.ToLower().Contains(query) ?? false);
        }

        private void ResetSearch()
        {
            SearchText = "";
            FilteredDatabases.Clear();
            foreach (var db in Databases)
                FilteredDatabases.Add(db);
        }

       
        private void AddDatabase()
        {
            ErrorMessage = ""; 

            if (string.IsNullOrWhiteSpace(Provider))
            {
                ErrorMessage = "Provider je obavezan.";
                return;
            }
            if (string.IsNullOrWhiteSpace(QueryLanguage))
            {
                ErrorMessage = "Query Language je obavezan.";
                return;
            }
            var db = new Database(Provider, Type, QueryLanguage, null, null, State);
            Databases.Add(db);
            FilteredDatabases.Add(db);
            ClearFields();
        }

        private void EditDatabase()
        {

            if (string.IsNullOrWhiteSpace(Provider))
            {
                System.Windows.MessageBox.Show("Provider je obavezan!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(QueryLanguage))
            {
                System.Windows.MessageBox.Show("Query Language je obavezan!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (SelectedDatabase == null) return;
            SelectedDatabase.Provider = Provider;
            SelectedDatabase.Type = Type;
            SelectedDatabase.QueryLanguage = QueryLanguage;
            SelectedDatabase.State = State;
            IsEditVisible = false;
            SelectedDatabase = null;
            OnPropertyChanged(nameof(Databases));
            OnPropertyChanged(nameof(FilteredDatabases));
            ClearFields();
        }

        private void RemoveDatabase()
        {
            if (SelectedDatabase == null) return;
            Databases.Remove(SelectedDatabase);
            FilteredDatabases.Remove(SelectedDatabase);
            SelectedDatabase = null;
            ClearFields();
        }


        public bool CanSaveOrEdit()
        {
            return !string.IsNullOrWhiteSpace(Provider)
                && !string.IsNullOrWhiteSpace(QueryLanguage);
        }
        private void ClearFields()
        {
            Provider = "";
            QueryLanguage = "";
            Type = DatabaseType.RELATIONAL;
            State = DatabaseState.Online;
        }

        private void LoadEditFields()
        {
            if (SelectedDatabase == null) { ClearFields(); return; }
            Provider = SelectedDatabase.Provider;
            QueryLanguage = SelectedDatabase.QueryLanguage;
            Type = SelectedDatabase.Type;
            State = SelectedDatabase.State;
        }
        private void EditRow(Database db)
        {
            if (db == null) return;
            SelectedDatabase = db;
            Provider = db.Provider;
            Type = db.Type;
            QueryLanguage = db.QueryLanguage;
            State = db.State;
            IsEditVisible = true;
        }

        private void RemoveRow(Database db)
        {
            if (db == null) return;
            Databases.Remove(db);
            FilteredDatabases.Remove(db);
            if (SelectedDatabase == db)
                SelectedDatabase = null;
            ClearFields();
        }
        #endregion
    }
}
