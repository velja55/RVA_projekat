using Projekat18.MapperHelper;
using Projekat18.Model;
using Projekat18.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat18.Command
{
    public class RemoveDatabaseCommand : DataBaseCommand
    {
        private DatabaseViewModel _viewModel;
        private Database _database;
        private int _removedIndex;
       
        private ObservableCollection<Table> _tables;
        private List<Table> _removedTables = new List<Table>();


        public RemoveDatabaseCommand(DatabaseViewModel viewModel, Database database, ObservableCollection<Table> tables)
        {
            _viewModel = viewModel;
            _database = database;
            _removedIndex = _viewModel.Databases.IndexOf(_database);
            _tables = tables;
        }

        public override void Execute()
        {
            if (_database == null)
            {
                _viewModel.ErrorMessage = "No database selected for removal!";
                return;
            }

            _viewModel.Databases.Remove(_database);
            _viewModel.FilteredDatabases.Remove(_database);
            _viewModel.proxy.DeleteDatabase(_database.Provider);
            _viewModel.SelectedDatabase = null;
            if (_database.Tables != null)
            {
                foreach (var t in _database.Tables)
                {
                    if (_tables.Contains(t))
                    {
                        _removedTables.Add(t); // za undo
                        _tables.Remove(t);
                    }
                }
            }


        }

        public override void Undo()
        {
            // Vraćamo bazu nazad na staru poziciju radi Undo
            if (_removedIndex >= 0 && _removedIndex <= _viewModel.Databases.Count)
                _viewModel.Databases.Insert(_removedIndex, _database);
            else
                _viewModel.Databases.Add(_database);

            _viewModel.FilteredDatabases.Add(_database);
            _viewModel.proxy.AddDatabase(DatabaseMapper.FromModel(_database));
            foreach (var t in _removedTables)
            {
                if (!_tables.Contains(t))
                    _tables.Add(t);
            }

        }
    }
}
