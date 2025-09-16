using Contracts;
using Projekat18.Helpers;
using Projekat18.MapperHelper;
using Projekat18.Model;
using Projekat18.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Administrator = Projekat18.Model.Administrator;
using Table = Projekat18.Model.Table;
using log4net;
using log4net.Config;

namespace Projekat18.ViewModel
{
    public class AddTableViewModel : BaseViewModel
    {
        private static string _address = "http://localhost:8080/DatabaseService";
        IDatabaseService proxy;

        private static readonly ILog log = LogManager.GetLogger(typeof(AddTableViewModel));

        public string TableName { get; set; }
        public string ColumnsText { get; set; }

        public MyICommand AddCommand { get; }
        public MyICommand CancelCommand { get; }

        private UserViewModel _parent;
        private Database _db;
        private Administrator admin;
        private ObservableCollection<Database> _databases;

        public AddTableViewModel(UserViewModel parent, Database db, Administrator admin, ObservableCollection<Database> databases)
        {
            proxy = CreateChannel();
            _parent = parent;
            _db = db;
            this.admin = admin;
            this._databases = databases;
            AddCommand = new MyICommand(OnAdd);
            CancelCommand = new MyICommand(OnCancel);

            XmlConfigurator.Configure();
            log.Info($"AddTableViewModel initialized for database '{_db?.Provider ?? "Unknown"}'.");
        }

        private void OnAdd()
        {
            try
            {
                var columns = (ColumnsText ?? "").Split(',')
                                .Select(s => s.Trim())
                                .Where(s => !string.IsNullOrWhiteSpace(s))
                                .ToList();

                if (string.IsNullOrWhiteSpace(TableName) || columns.Count == 0)
                {
                    log.Warn("Attempt to add a table failed - table name or columns are empty.");
                    return;
                }

                _db.Tables.Add(new Table(TableName, columns));
                proxy.UpdateDatabase(DatabaseMapper.FromModel(_db), _db.Provider);

                log.Info($"Table '{TableName}' added to database '{_db.Provider}' with {columns.Count} columns.");
                _parent.CurrentView = new DatabaseView(_parent, admin, _databases);
            }
            catch (Exception ex)
            {
                log.Error($"Error while adding table '{TableName}': {ex.Message}", ex);
            }
        }

        private IDatabaseService CreateChannel()
        {
            try
            {
                var binding = new BasicHttpBinding();
                var endpoint = new EndpointAddress(_address);

                ChannelFactory<IDatabaseService> factory = new ChannelFactory<IDatabaseService>(binding, endpoint);
                IDatabaseService proxy = factory.CreateChannel();

                log.Info("WCF channel successfully created in AddTableViewModel.");
                return proxy;
            }
            catch (Exception ex)
            {
                log.Error($"[ERROR] Failed to create WCF channel: {ex.Message}", ex);
                throw;
            }
        }

        private void OnCancel()
        {
            log.Info("Add table canceled by user.");
            _parent.CurrentView = new DatabaseView(_parent, admin, _databases);
        }
    }
}
