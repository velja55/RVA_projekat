using Contracts;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WcfDatabaseServer.Services
{
    public class CsvStorage : IDataStorage
    {
        private readonly string dbFile = "databases.csv";
        private readonly string tablesFile = "databases_tables.csv";
        private readonly string legacyFile = "legacy_databases.csv";
        private static readonly ILog log = LogManager.GetLogger(typeof(CsvStorage));

        public List<DataBase> LoadDatabases()
        {
            try
            {
                log.Info("[INFO] Loading databases.");
                var result = new List<DataBase>();

                if (!File.Exists(dbFile)) return result;

                foreach (var line in File.ReadAllLines(dbFile))
                {
                    var parts = line.Split(';');
                    if (parts.Length < 5) continue;

                    var db = new DataBase
                    {
                        Provider = parts[0],
                        Type = Enum.TryParse(parts[1], out DatabaseType t) ? t : DatabaseType.RELATIONAL,
                        QueryLanguage = parts[2],
                        State = Enum.TryParse(parts[3], out DatabaseState s) ? s : DatabaseState.Offline,
                        Admin = new Administrator
                        {
                            UserName = parts[4],
                            Password = parts.Length > 5 ? parts[5] : "",
                            Certificate = parts.Length > 6 ? parts[6] : "",
                            Permissions = parts.Length > 7 ? parts[7] : ""
                        },
                        Tables = LoadTables(parts[0])
                    };

                    result.Add(db);
                }

                return result;
            }
            catch (Exception ex)
            {
                log.Error($"[ERROR] {ex.Message}");
                return new List<DataBase>();
            }
        }

        public List<LegacyDatabase> LoadLegacyDatabases()
        {
            try
            {
                log.Info("[INFO] Loading legacy databases.");
                var result = new List<LegacyDatabase>();

                if (!File.Exists(legacyFile)) return result;

                foreach (var line in File.ReadAllLines(legacyFile))
                {
                    var parts = line.Split(';');
                    if (parts.Length < 3) continue;

                    result.Add(new LegacyDatabase
                    {
                        DBSystemName = parts[0],
                        ApproachToStoring = parts[1],
                        InstructionSyntax = parts[2]
                    });
                }

                return result;
            }
            catch (Exception ex)
            {
                log.Error($"[ERROR] {ex.Message}");
                return new List<LegacyDatabase>();
            }
        }

        public void SaveDatabases(List<DataBase> dbs)
        {
            try
            {
                log.Info("[INFO] Saving databases.");
                var lines = dbs.Select(db =>
                    $"{db.Provider};{db.Type};{db.QueryLanguage};{db.State};" +
                    $"{db.Admin?.UserName};{db.Admin?.Password};{db.Admin?.Certificate};{db.Admin?.Permissions}"
                ).ToList();

                File.WriteAllLines(dbFile, lines);

                SaveAllTables(dbs);
            }
            catch (Exception ex)
            {
                log.Error($"[ERROR] {ex.Message}");
            }
        }

        public void SaveLegacyDatabases(List<LegacyDatabase> legacies)
        {
            try
            {
                log.Info("[INFO] Saving legacy databases.");
                var lines = legacies.Select(l =>
                    $"{l.DBSystemName};{l.ApproachToStoring};{l.InstructionSyntax}"
                ).ToList();

                File.WriteAllLines(legacyFile, lines);
            }
            catch (Exception ex)
            {
                log.Error($"[ERROR] {ex.Message}");
            }
        }

        private List<Table> LoadTables(string dbName)
        {
            try
            {
                log.Info("[INFO] Loading tables.");
                var result = new List<Table>();

                if (!File.Exists(tablesFile)) return result;

                foreach (var line in File.ReadAllLines(tablesFile))
                {
                    var parts = line.Split(';');
                    if (parts.Length < 3) continue;

                    if (parts[0] == dbName)
                    {
                        result.Add(new Table
                        {
                            Name = parts[1],
                            ColumnHeaders = parts[2].Split(',').ToList()
                        });
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                log.Error($"[ERROR] {ex.Message}");
                return new List<Table>();
            }
        }

        private void SaveAllTables(List<DataBase> dbs)
        {
            try
            {
                log.Info("[INFO] Saving tables.");
                var lines = new List<string>();

                foreach (var db in dbs)
                {
                    foreach (var table in db.Tables ?? new List<Table>())
                    {
                        lines.Add($"{db.Provider};{table.Name};{string.Join(",", table.ColumnHeaders)}");
                    }
                }

                File.WriteAllLines(tablesFile, lines);
            }
            catch (Exception ex)
            {
                log.Error($"[ERROR] {ex.Message}");
            }
        }
    }
}
