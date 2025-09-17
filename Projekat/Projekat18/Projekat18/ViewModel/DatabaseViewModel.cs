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
using Projekat18.DBStates;
using Projekat18.Adapter;
using Contracts;
using System.Net;
using System.ServiceModel;
using Administrator = Projekat18.Model.Administrator;
using DatabaseType = Projekat18.Model.Enums.DatabaseType;
using LegacyDatabase = Projekat18.Model.LegacyDatabase;
using Table = Projekat18.Model.Table;
using Projekat18.MapperHelper;
using log4net;
using log4net.Config;
using Projekat18.Command;

namespace Projekat18.ViewModel
{
    public class DatabaseViewModel : BaseViewModel
    {
        #region Fields and Properties
        public ObservableCollection<Database> Databases { get; set; }

        private static string _address = "http://localhost:8081/DatabaseService";
        public IDatabaseService proxy;

        CommandManager commandManager;

        private static readonly ILog log = LogManager.GetLogger(typeof(DatabaseViewModel));

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
            set
            {
                _state = value;
                OnPropertyChanged(nameof(State));
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

        public DatabaseViewModel(UserViewModel parent, Administrator u, ObservableCollection<Database> databases)
        {
            proxy = CreateChannel();
            XmlConfigurator.Configure();

            Databases = databases;
            CurrentUserName = u.UserName;
            CanAddDatabase = u.Permissions?.Contains("Add") ?? false;
            admin = u;
            FilteredDatabases = new ObservableCollection<Database>(Databases);
            ErrorMessage = "";

            SearchCommand = new MyICommand(SearchDatabases);
            ResetSearchCommand = new MyICommand(ResetSearch);
            EditRowCommand = new MyICommand<Database>(EditRow);
            RemoveRowCommand = new MyICommand<Database>(RemoveRow);
            CancelEditCommand = new MyICommand(() => { IsEditVisible = false; ClearFields(); SelectedDatabase = null; });
            ShowTablesCommand = new MyICommand<Database>(ShowTablesForDatabase);
            ShowAddTableViewCommand = new MyICommand<Database>(ShowAddTableView);
            AddLegacyDatabaseCommand = new MyICommand(AddLegacyDatabase);
            commandManager= new CommandManager();
            _parent = parent;
        }

        private IDatabaseService CreateChannel()
        {
            var binding = new BasicHttpBinding();
            var endpoint = new EndpointAddress(_address);

            ChannelFactory<IDatabaseService> factory = new ChannelFactory<IDatabaseService>(binding, endpoint);

            IDatabaseService proxy = factory.CreateChannel();

            return proxy;
        }

        #region Methods

        private void AddLegacyDatabase()
        {
            LegacyErrorMessage = "";

            var legacyDb = new LegacyDatabase
            {
                DbSystemName = LegacyDbSystemName,
                InstructionSyntax = LegacyInstructionSyntax,
                NumberOfTables = LegacyNumberOfTables,
                StorageAdmin = CurrentUserName
            };

            proxy.AddLegacyDatabase(LegacyDatabaseMapper.FromModel(legacyDb));

            var adapter = new LegacyDigitalStorageAdapter(legacyDb, LegacyStateString);

            Databases.Add(adapter);
            proxy.AddDatabase(DatabaseMapper.FromModel(adapter));
            FilteredDatabases.Add(adapter);

            log.Info($"Legacy database added: {legacyDb.DbSystemName}");

            LegacyDbSystemName = "";
            LegacyInstructionSyntax = "";
            LegacyNumberOfTables = 0;
            LegacyStorageAdmin = "";
            LegacyStateString = "";
        }

        private void ShowAddTableView(Database db)
        {
            _parent.CurrentView = new AddTableView(_parent, db, admin, Databases);
            log.Info($"Opened AddTableView for database: {db?.Provider}");
        }

        private void ShowTablesForDatabase(Database db)
        {
            if (db == null || db.Tables == null) return;

            var tableView = new TableView(new ObservableCollection<Table>(db.Tables));
            _parent.CurrentView = tableView;

            log.Info($"Tables displayed for database: {db.Provider}");
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
            log.Info($"Search executed. Query: '{query}', results: {FilteredDatabases.Count}");
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
            log.Info("Search reset.");
        }

        public void OnAddDatabase()
        {
            ErrorMessage = "";

            if (string.IsNullOrWhiteSpace(Provider))
            {
                ErrorMessage = "Provider is required.";
                return;
            }
            if (string.IsNullOrWhiteSpace(QueryLanguage))
            {
                ErrorMessage = "Query Language is required.";
                return;
            }

            var db = new Database(Provider, Type, QueryLanguage, null, admin, State);
            var cmd = new AddDatabaseCommand(this, db);
            commandManager.ExecuteCommand(cmd);
            //_parent.PushUndo(cmd);
            ClearFields();
        }

        public void OnEditDatabase()
        {
            ErrorMessage = "";

            if (SelectedDatabase == null)
            {
                ErrorMessage = "No database selected for edit!";
                return;
            }
            if (string.IsNullOrWhiteSpace(Provider))
            {
                ErrorMessage = "Provider is required!";
                return;
            }
            if (string.IsNullOrWhiteSpace(QueryLanguage))
            {
                ErrorMessage = "Query Language is required!";
                return;
            }

            var cmd = new EditDatabaseCommand(this, SelectedDatabase, Provider, Type, QueryLanguage, State);
            commandManager.ExecuteCommand(cmd);
            IsEditVisible = false;
            SelectedDatabase = null;
            ClearFields();
        }

        public void OnRemoveDatabase(Database SelectedDatabase)
        {
            if (SelectedDatabase == null)
            {
                ErrorMessage = "No database selected for removal!";
                return;
            }

            var cmd = new RemoveDatabaseCommand(this, SelectedDatabase);
            commandManager.ExecuteCommand(cmd);
            //_parent.PushUndo(cmd);
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
            string oldProvider = db.Provider;
            Provider = db.Provider;
            Type = db.Type;
            QueryLanguage = db.QueryLanguage;
            State = db.State;
            IsEditVisible = true;

            proxy.UpdateDatabase(DatabaseMapper.FromModel(db), oldProvider);
            log.Info($"Opened database edit: {db.Provider}");
        }

        private void RemoveRow(Database db)
        {
            if (db == null) return;

            proxy.DeleteDatabase(db.Provider);
            Databases.Remove(db);
            FilteredDatabases.Remove(db);

            log.Info($"Database removed: {db.Provider}");

            foreach (Table t in db.Tables)
            {
                _parent.tables.Remove(t);
            }

            _parent.PushUndo(() =>
            {
                Databases.Add(db);
                proxy.AddDatabase(DatabaseMapper.FromModel(db));
                FilteredDatabases.Add(db);
                foreach (Table t in db.Tables)
                    _parent.tables.Add(t);
                log.Info($"Undo database removal: {db.Provider}");
                OnPropertyChanged(nameof(Databases));
                OnPropertyChanged(nameof(FilteredDatabases));
            });

            _parent.PushRedo(() =>
            {
                Databases.Remove(db);
                proxy.DeleteDatabase(db.Provider);
                FilteredDatabases.Remove(db);
                log.Info($"Redo database removal: {db.Provider}");
                OnPropertyChanged(nameof(Databases));
                OnPropertyChanged(nameof(FilteredDatabases));
            });

            if (SelectedDatabase == db)
                SelectedDatabase = null;
            ClearFields();
        }

        public void Undo() => commandManager.Undo();
        public void Redo() => commandManager.Redo();

        #endregion
    }
}
