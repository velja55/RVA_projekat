using Contracts;
using Projekat18.DBStates;
using Projekat18.Helpers;
using Projekat18.MapperHelper;
using Projekat18.Model;
using Projekat18.Model.Enums;
using Projekat18.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Projekat18.Command;


using Administrator = Projekat18.Model.Administrator;
using Table = Projekat18.Model.Table;
using CommandManager = Projekat18.Command.CommandManager;

namespace Projekat18.ViewModel
{
    public  class UserViewModel : BaseViewModel
    {
        private static string _address = "http://localhost:8081/DatabaseService";

        public CommandManager commandManager;

        private object _currentView;
        public object CurrentView
        {
            get => _currentView;
            set { _currentView = value; OnPropertyChanged(nameof(CurrentView)); }
        }

        public MyICommand UndoCommand { get; }
        public MyICommand RedoCommand { get; }

        public MyICommand ShowDatabaseViewCommand { get; }
        public MyICommand ShowTableViewCommand { get; }
        public MyICommand ShowLegacyViewCommand { get; }

        public ObservableCollection<Table> tables = new ObservableCollection<Table>();
        public MyICommand ShowStateChartViewCommand { get; }
        public UserViewModel(Administrator administrator)
        {
            var binding = new BasicHttpBinding();
            var endpoint = new EndpointAddress(_address);

            ChannelFactory<IDatabaseService> factory = new ChannelFactory<IDatabaseService>(binding, endpoint);

            IDatabaseService proxy = factory.CreateChannel();

            var dbs = proxy.GetDatabases();

            ObservableCollection<Database> Databases = new ObservableCollection<Database>();

            foreach (var d in dbs)
            {
                Databases.Add(DatabaseMapper.FromContract(d));
            }

            commandManager = new CommandManager();
            CurrentView = new DatabaseView(this,administrator,Databases);
            
            foreach (Database db in Databases) {
                foreach (Table t in db.Tables)
                {
                    tables.Add(t);
                }
            }
            ShowDatabaseViewCommand = new MyICommand(() => CurrentView = new DatabaseView(this,administrator,Databases));
            ShowTableViewCommand=new MyICommand(() => CurrentView = new TableView(tables));
            ShowStateChartViewCommand = new MyICommand(()=>CurrentView=new StateChartView(Databases));
            UndoCommand = new MyICommand(Undo);
            RedoCommand = new MyICommand(Redo);
        }

       

        private void Undo()
        {
            commandManager.Undo();
        }

        private void Redo()
        {
            commandManager.Redo();
        }



    }
}
