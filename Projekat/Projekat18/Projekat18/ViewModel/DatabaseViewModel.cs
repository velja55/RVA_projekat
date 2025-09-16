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
using Projekat18.View;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using Projekat18.DBStates;
using Projekat18.Adapter;

namespace Projekat18.ViewModel
{
    public class DatabaseViewModel : BaseViewModel
    {
        #region Fields and Properties
        public ObservableCollection<Database> Databases { get; set; }
        public ObservableCollection<Database> FilteredDatabases { get; set; }

        private Administrator admin;

        private string _searchText;
        public string CurrentUserName { get; set; }
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

        private IDatabaseState _state;
        public string StateColor => State?.Color;
        public IDatabaseState State
        {
            get => _state;
            set { _state = value; OnPropertyChanged(nameof(State));
                
                OnPropertyChanged(nameof(StateColor));
            }
        }

        private Database _selectedDatabase;
        public Database SelectedDatabase
        {
            get => _selectedDatabase;
            set { _selectedDatabase = value; OnPropertyChanged(nameof(SelectedDatabase)); LoadEditFields(); }
        }


        private bool _canAddDatabase;
        public bool CanAddDatabase
        {
            get => _canAddDatabase;
            set { _canAddDatabase = value; OnPropertyChanged(nameof(CanAddDatabase)); }
        }
        private UserViewModel _parent;

        private string _legacyDbSystemName;
        public string LegacyDbSystemName
        {
            get => _legacyDbSystemName;
            set { _legacyDbSystemName = value; OnPropertyChanged(nameof(LegacyDbSystemName)); }
        }

        private string _legacyInstructionSyntax;
        public string LegacyInstructionSyntax
        {
            get => _legacyInstructionSyntax;
            set { _legacyInstructionSyntax = value; OnPropertyChanged(nameof(LegacyInstructionSyntax)); }
        }

        private int _legacyNumberOfTables;
        public int LegacyNumberOfTables
        {
            get => _legacyNumberOfTables;
            set { _legacyNumberOfTables = value; OnPropertyChanged(nameof(LegacyNumberOfTables)); }
        }

        // Opcionalno: poruka za grešku
        private string _legacyErrorMessage;
        public string LegacyErrorMessage
        {
            get => _legacyErrorMessage;
            set { _legacyErrorMessage = value; OnPropertyChanged(nameof(LegacyErrorMessage)); }
        }

        private string _legacyStateString;
        public string LegacyStateString
        {
            get => _legacyStateString;
            set { _legacyStateString = value; OnPropertyChanged(nameof(LegacyStateString)); }
        }

        public string LegacyStorageAdmin { get; set; }


        public ObservableCollection<string> LegacyStates { get; set; } = new ObservableCollection<string>
        {
            "Online", "Offline", "Recovering", "Restoring"
        };

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
        public MyICommand<Database> ShowTablesCommand { get; }
        public MyICommand<Database> ShowAddTableViewCommand { get; }

        public MyICommand AddLegacyDatabaseCommand { get; }
        #endregion
        public DatabaseViewModel(UserViewModel parent,Administrator u,ObservableCollection<Database> databases)
        {
            Databases = databases;
            CurrentUserName = u.UserName;
            CanAddDatabase = u.Permissions?.Contains("Add") ?? false;
            admin = u;
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
            ShowTablesCommand = new MyICommand<Database>(ShowTablesForDatabase);
            ShowAddTableViewCommand = new MyICommand<Database>(ShowAddTableView);
            AddLegacyDatabaseCommand = new MyICommand(AddLegacyDatabase);
            _parent = parent;
        }

        #region Methods

        private void AddLegacyDatabase()
        {
            LegacyErrorMessage = "";

            // Validacija kao pre...

            // LegacyDatabase bez StateString!
            var legacyDb = new LegacyDatabase
            {
                DbSystemName = LegacyDbSystemName,
                InstructionSyntax = LegacyInstructionSyntax,
                NumberOfTables = LegacyNumberOfTables,
                StorageAdmin = LegacyStorageAdmin
            };

            // State unosiš iz VM-a, npr. iz ComboBoxa
            var adapter = new LegacyDigitalStorageAdapter(legacyDb, LegacyStateString);

            Databases.Add(adapter);
            FilteredDatabases.Add(adapter);

            // Resetuj polja
            LegacyDbSystemName = "";
            LegacyInstructionSyntax = "";
            LegacyNumberOfTables = 0;
            LegacyStorageAdmin = "";
            LegacyStateString = ""; // ili podesi default
        }

        private void ShowAddTableView(Database db)
        {
            // Pretpostavljam da imaš UserViewModel kao parent
            _parent.CurrentView = new AddTableView(_parent, db,admin,Databases);
        }
        private void ShowTablesForDatabase(Database db)
        {
            if (db == null || db.Tables == null) return;
            // Otvori novi View za TableView i prosledi tabele
            var tableView = new TableView(new ObservableCollection<Table>(db.Tables));
            // Primer: Ako imaš UserViewModel i CurrentView property:
            // (Pretpostavka da postoji CurrentView kao u ranijem kodu)
            _parent.CurrentView = tableView;
            // Ako si u DatabaseViewModel, trebaš referencu na parent viewmodel ili event
        }

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
                || (db.State.ToString().ToLower().Contains(query))
                || (db.Admin?.UserName?.ToLower().Contains(query) ?? false);
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
            var db = new Database(Provider, Type, QueryLanguage, null, admin, State);
            Databases.Add(db);
            FilteredDatabases.Add(db);
            if (db.Tables.Count > 0) {
                foreach (Table t in db.Tables)
                    _parent.tables.Add(t);
            }
            _parent.PushUndo(() => {
                Databases.Remove(db);
                FilteredDatabases.Remove(db);
            });
            _parent.PushRedo(() => {
                Databases.Add(db);
                FilteredDatabases.Add(db);
            });


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

            // --- Pamti staro stanje ---
            var db = SelectedDatabase;
            string oldProvider = db.Provider;
            DatabaseType oldType = db.Type;
            string oldQueryLang = db.QueryLanguage;
            IDatabaseState oldState = db.State;

            // --- Pamti novo stanje ---
            string newProvider = Provider;
            DatabaseType newType = Type;
            string newQueryLang = QueryLanguage;
            IDatabaseState newState = State;

            // --- Izvrši izmenu ---
            db.Provider = newProvider;
            db.Type = newType;
            db.QueryLanguage = newQueryLang;
            db.State = newState;

            // --- Undo vrati staro ---
            _parent.PushUndo(() =>
            {
                db.Provider = oldProvider;
                db.Type = oldType;
                db.QueryLanguage = oldQueryLang;
                db.State = oldState;
                OnPropertyChanged(nameof(Databases));
                OnPropertyChanged(nameof(FilteredDatabases));
            });

            // --- Redo ponovi novo ---
            _parent.PushRedo(() =>
            {
                db.Provider = newProvider;
                db.Type = newType;
                db.QueryLanguage = newQueryLang;
                db.State = newState;
                OnPropertyChanged(nameof(Databases));
                OnPropertyChanged(nameof(FilteredDatabases));
            });

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
            State = new OnlineState();
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
            foreach(Table t in db.Tables)
            {
                _parent.tables.Remove(t);

            }


            _parent.PushUndo(() => {
                Databases.Add(db);
                FilteredDatabases.Add(db);
                foreach (Table t in db.Tables)
                    _parent.tables.Add(t);
                OnPropertyChanged(nameof(Databases));
                OnPropertyChanged(nameof(FilteredDatabases));
            });

            _parent.PushRedo(() => {
                Databases.Remove(db);
                FilteredDatabases.Remove(db);
                OnPropertyChanged(nameof(Databases));
                OnPropertyChanged(nameof(FilteredDatabases));
            });

            if (SelectedDatabase == db)
                SelectedDatabase = null;
            ClearFields();
        }
        #endregion
    }
}
