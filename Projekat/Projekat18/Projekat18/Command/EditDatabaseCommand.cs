using Projekat18.MapperHelper;
using Projekat18.Model;
using Projekat18.Model.Enums;
using Projekat18.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat18.Command
{
    public class EditDatabaseCommand : DataBaseCommand
    {
        private DatabaseViewModel _viewModel;
        private Database _database;
        private string _oldProvider;
        private DatabaseType _oldType;
        private string _oldQueryLang;
        private IDatabaseState _oldState;

        private string _newProvider;
        private DatabaseType _newType;
        private string _newQueryLang;
        private IDatabaseState _newState;

        public EditDatabaseCommand(DatabaseViewModel viewModel, Database database, string newProvider, DatabaseType newType, string newQueryLang, IDatabaseState newState)
        {
            _viewModel = viewModel;
            _database = database;
            _oldProvider = database.Provider;
            _oldType = database.Type;
            _oldQueryLang = database.QueryLanguage;
            _oldState = database.State;

            _newProvider = newProvider;
            _newType = newType;
            _newQueryLang = newQueryLang;
            _newState = newState;
        }

        public override void Execute()
        {
            // Validacija pre izmene
            _viewModel.ErrorMessage = "";
            if (string.IsNullOrWhiteSpace(_newProvider))
            {
                _viewModel.ErrorMessage = "Provider is required!";
                return;
            }
            if (string.IsNullOrWhiteSpace(_newQueryLang))
            {
                _viewModel.ErrorMessage = "Query Language is required!";
                return;
            }
            if (_database == null)
            {
                _viewModel.ErrorMessage = "No database selected for edit!";
                return;
            }

            _database.Provider = _newProvider;
            _database.Type = _newType;
            _database.QueryLanguage = _newQueryLang;
            _database.State = _newState;
            _viewModel.proxy.UpdateDatabase(DatabaseMapper.FromModel(_database), _oldProvider);
            
        }

        public override void Undo()
        {
            _database.Provider = _oldProvider;
            _database.Type = _oldType;
            _database.QueryLanguage = _oldQueryLang;
            _database.State = _oldState;
            _viewModel.proxy.UpdateDatabase(DatabaseMapper.FromModel(_database), _database.Provider);
           
        }
    }
}
