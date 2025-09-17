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
    public class RemoveDatabaseCommand : ICommand
    {
        private DatabaseViewModel _viewModel;
        private Database _database;
        private int _removedIndex;

        public RemoveDatabaseCommand(DatabaseViewModel viewModel, Database database)
        {
            _viewModel = viewModel;
            _database = database;
            _removedIndex = _viewModel.Databases.IndexOf(_database);
        }

        public void Execute()
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
           // _viewModel.ClearFields();
        }

        public void Undo()
        {
            // Vraćamo bazu nazad na staru poziciju radi Undo
            if (_removedIndex >= 0 && _removedIndex <= _viewModel.Databases.Count)
                _viewModel.Databases.Insert(_removedIndex, _database);
            else
                _viewModel.Databases.Add(_database);

            _viewModel.FilteredDatabases.Add(_database);
            _viewModel.proxy.AddDatabase(DatabaseMapper.FromModel(_database));
        }
    }
}
