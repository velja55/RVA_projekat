using Projekat18.Adapter;
using Projekat18.MapperHelper;
using Projekat18.Model;
using Projekat18.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat18.Command
{
    public class AddDatabaseCommand : DataBaseCommand
    {
        private DatabaseViewModel _viewModel;
        private Database _database;
        private bool _isLegacy;
        private LegacyDatabase _legacyDatabase;
        private string _legacyStateString;

        public AddDatabaseCommand(DatabaseViewModel viewModel, Database database)
        {
            _viewModel = viewModel;
            _database = database;
            _isLegacy = false;
        }

        public AddDatabaseCommand(DatabaseViewModel viewModel, LegacyDatabase legacyDatabase, string legacyStateString)
        {
            _viewModel = viewModel;
            _legacyDatabase = legacyDatabase;
            _legacyStateString = legacyStateString;
            _isLegacy = true;
        }

        public override void Execute()
        {
            if (_isLegacy)
            {
                var adapter = new LegacyDigitalStorageAdapter(_legacyDatabase, _legacyStateString);
                _viewModel.Databases.Add(adapter);
                _viewModel.proxy.AddDatabase(DatabaseMapper.FromModel(adapter));
                _viewModel.FilteredDatabases.Add(adapter);
                foreach (Table t in adapter.Tables)
                {
                    _viewModel._parent.tables.Add(t);
                }
            }
            else
            {
                _database.State.Handle(_database);
                _viewModel.Databases.Add(_database);
                foreach (var t in _database.Tables)
                {
                    _viewModel._parent.tables.Add(t);
                }
                _viewModel.proxy.AddDatabase(DatabaseMapper.FromModel(_database));
                _viewModel.FilteredDatabases.Add(_database);
            }
        }

        public override void Undo()
        {
            if (_isLegacy)
            {
                var adapter = _viewModel.Databases.OfType<LegacyDigitalStorageAdapter>()
                    .FirstOrDefault(x => x.Provider == _legacyDatabase.DbSystemName);
                if (adapter != null)
                {
                    _viewModel.Databases.Remove(adapter);
                    _viewModel.proxy.DeleteDatabase(adapter.Provider);
                    _viewModel.FilteredDatabases.Remove(adapter);
                }
            }
            else
            {
                _viewModel.Databases.Remove(_database);
                foreach (var t in _database.Tables)
                {
                    _viewModel._parent.tables.Remove(t);
                }
                _viewModel.proxy.DeleteDatabase(_database.Provider);
                _viewModel.FilteredDatabases.Remove(_database);
            }
        }
    }
}
