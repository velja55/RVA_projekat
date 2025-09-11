using Projekat18.Helpers;
using Projekat18.Model;
using Projekat18.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat18.ViewModel
{
    public class AddTableViewModel : BaseViewModel
    {
        public string TableName { get; set; }
        public string ColumnsText { get; set; }

        public MyICommand AddCommand { get; }
        public MyICommand CancelCommand { get; }

        private UserViewModel _parent;
        private Database _db;
        private Administrator admin;
        private ObservableCollection<Database> _databases;

        public AddTableViewModel(UserViewModel parent, Database db,Administrator admin,ObservableCollection<Database> databases)
        {
            _parent = parent;
            _db = db;
            this.admin = admin;
            this._databases = databases;
            AddCommand = new MyICommand(OnAdd);
            CancelCommand = new MyICommand(OnCancel);
        }

        private void OnAdd()
        {
            var columns = (ColumnsText ?? "").Split(',').Select(s => s.Trim()).Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
            if (string.IsNullOrWhiteSpace(TableName) || columns.Count == 0)
                return;
            _db.Tables.Add(new Table(TableName, columns));
            _parent.CurrentView = new DatabaseView(_parent,admin,_databases);
        }

        private void OnCancel() => _parent.CurrentView = new DatabaseView(_parent,admin,_databases);
    }
}
