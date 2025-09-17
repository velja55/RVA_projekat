using Contracts;
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WcfDatabaseServer.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class DatabaseService : IDatabaseService
    {
        private IDataStorage storage;
        private static string SelectedFormat = "XML";
        private static readonly ILog log = LogManager.GetLogger(typeof(DatabaseService));

        private static readonly Lazy<DatabaseService> _instance =
        new Lazy<DatabaseService>(() => new DatabaseService());

        public static DatabaseService Instance => _instance.Value;

        private DatabaseService()
        {
            SetStorageFormat(SelectedFormat);
            EnsureDefaultDatabases();
        }

        public static void ConfigureFormat(string format)
        {
            XmlConfigurator.Configure();

            log.Info("DatabaseService started.\n");

            SelectedFormat = format;
            log.Info("[CONFIGURATION] Setting storage format.\n");
        }

        public void SetStorageFormat(string format)
        {
            switch (format.ToUpper())
            {
                case "XML": storage = new XmlStorage(); break;
                case "CSV": storage = new CsvStorage(); break;
                case "JSON": storage = new JsonStorage(); break;
                default: storage = new XmlStorage(); break;
            }
            log.Info("[INFO] Storage format set.\n");
        }

        private void EnsureDefaultDatabases()
        {
            var dbs = storage.LoadDatabases();
            if (dbs.Count < 2)
            {
                log.Info("[INFO] Adding database instances.\n");
                List<Table> tables1 = new List<Table>
                {
                    new Table { Name = "Korisnici", ColumnHeaders = new List<string> { "Id", "Ime", "Prezime", "Email" }},
                    new Table { Name = "Porudzbine", ColumnHeaders = new List<string> { "PorudzbinaId", "KorisnikId", "Datum", "Ukupno" }},
                    new Table { Name = "Proizvodi", ColumnHeaders = new List<string> { "ProizvodId", "Naziv", "Cena", "Kategorija" }},
                    new Table { Name = "Kategorije", ColumnHeaders = new List<string> { "KategorijaId", "Naziv" }}
                };

                List<Table> tables2 = new List<Table>
                {
                    new Table { Name = "Korisnici2", ColumnHeaders = new List<string> { "Id2", "Ime", "Prezime", "Email" }},
                    new Table { Name = "Porudzbine2", ColumnHeaders = new List<string> { "PorudzbinaId2", "KorisnikId", "Datum", "Ukupno" }},
                    new Table { Name = "Proizvodi2", ColumnHeaders = new List<string> { "ProizvodId2", "Naziv", "Cena", "Kategorija" }},
                    new Table { Name = "Kategorije2", ColumnHeaders = new List<string> { "KategorijaId2", "Naziv" }}
                };
                dbs.Add(new DataBase
                {
                    Admin = new Administrator { Certificate = "Admin", UserName = "Stefan", Password = "Stefan", Permissions = "Add/Edit/Delete" },
                    Provider = "SQL Server",
                    QueryLanguage = "T-SQL",
                    State = DatabaseState.Online,
                    Type = DatabaseType.RELATIONAL,
                    Tables = tables1
                });
                dbs.Add(new DataBase
                {
                    Admin = new Administrator { Certificate = "Admin", UserName = "Veljko", Password = "Veljko", Permissions = "Add/Edit/Delete" },
                    Provider = "MongoDB",
                    QueryLanguage = "MongoDB",
                    State = DatabaseState.Offline,
                    Type = DatabaseType.NOSQL,
                    Tables = tables2
                });
                storage.SaveDatabases(dbs);
            }
        }

        public List<DataBase> GetDatabases()
        {
            log.Info("[INFO] Getting databases.\n");
            return storage.LoadDatabases();
        }
        public void AddDatabase(DataBase db)
        {
            var dbs = storage.LoadDatabases();
            dbs.Add(db);
            storage.SaveDatabases(dbs);
            log.Info($"[INFO] Adding new database. Provider: {db.Provider}.\n");
        }
        public void UpdateDatabase(DataBase db, string oldProvider)
        {
            var dbs = storage.LoadDatabases();
            var idx = dbs.FindIndex(d => d.Provider == oldProvider);
            if (idx >= 0) dbs[idx] = db;
            storage.SaveDatabases(dbs);
            log.Info($"[INFO] Updating database. Old provider: {oldProvider}, new provider: {db.Provider}.\n");
        }
        public void DeleteDatabase(string dbName)
        {
            var dbs = storage.LoadDatabases();
            dbs.RemoveAll(d => d.Provider == dbName);
            storage.SaveDatabases(dbs);
            log.Info($"[INFO] Deleting database. Provider: {dbName}.\n");
        }
        public List<Table> GetTables(string dbName)
        {
            log.Info($"[INFO] Getting tables for database. Provider: {dbName}.\n");
            var db = storage.LoadDatabases().Find(d => d.Provider == dbName);
            return db?.Tables ?? new List<Table>();
        }
        public void AddTable(string dbName, Table tbl)
        {
            var dbs = storage.LoadDatabases();
            var db = dbs.Find(d => d.Provider == dbName);
            db?.Tables.Add(tbl);
            storage.SaveDatabases(dbs);
            log.Info($"[INFO] Adding table in database. Provider: {dbName}.\n");
        }

        public List<LegacyDatabase> GetLegacyDatabases() => storage.LoadLegacyDatabases();
        public void AddLegacyDatabase(LegacyDatabase db)
        {
            var legacies = storage.LoadLegacyDatabases();
            legacies.Add(db);
            storage.SaveLegacyDatabases(legacies);
            log.Info($"[INFO] Adding legacy database. Provider: {db.DBSystemName}.\n");
        }
        public Dictionary<DatabaseState, int> GetStateCounts()
        {
            var dbs = storage.LoadDatabases();
            var dict = new Dictionary<DatabaseState, int>();

            foreach (DatabaseState state in Enum.GetValues(typeof(DatabaseState)))
            {
                dict[state] = dbs.Count(d => d.State == state);
            }

            log.Info($"[INFO] Getting state counts.\n");

            return dict;
        }
    }
}
