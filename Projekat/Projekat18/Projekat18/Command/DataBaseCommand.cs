using Projekat18.Model;
using Projekat18.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat18.Command
{
    public abstract class DataBaseCommand:ICommand
    {
        private DatabaseViewModel _viewModel;
        private Database _database;

        public abstract void Execute();

        public abstract void Undo();
    }
}
