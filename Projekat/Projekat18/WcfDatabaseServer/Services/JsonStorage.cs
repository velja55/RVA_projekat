using Contracts;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WcfDatabaseServer.Services
{
    public class JsonStorage : IDataStorage
    {
        private readonly string dbFile = "dbs.json";
        private readonly string legacyFile = "legacies.json";
        private static readonly ILog log = LogManager.GetLogger(typeof(JsonStorage));

        public List<DataBase> LoadDatabases()
        {
            try
            {
                log.Info("[INFO] Loading databases.");
                if (!File.Exists(dbFile)) return new List<DataBase>();
                var json = File.ReadAllText(dbFile);
                return JsonSerializer.Deserialize<List<DataBase>>(json) ?? new List<DataBase>();
            }
            catch (Exception ex)
            {
                log.Error($"[ERROR] {ex.Message}");
                return new List<DataBase>();
            }
        }

        public void SaveDatabases(List<DataBase> dbs)
        {
            try
            {
                log.Info("[INFO] Saving databases.");
                var json = JsonSerializer.Serialize(dbs, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(dbFile, json);
            }
            catch (Exception ex)
            {
                log.Error($"[ERROR] {ex.Message}");
            }
        }

        public List<LegacyDatabase> LoadLegacyDatabases()
        {
            try
            {
                log.Info("[INFO] Loading legacy databases.");
                if (!File.Exists(legacyFile)) return new List<LegacyDatabase>();
                var json = File.ReadAllText(legacyFile);
                return JsonSerializer.Deserialize<List<LegacyDatabase>>(json) ?? new List<LegacyDatabase>();
            }
            catch (Exception ex)
            {
                log.Error($"[ERROR] {ex.Message}");
                return new List<LegacyDatabase>();
            }
        }

        public void SaveLegacyDatabases(List<LegacyDatabase> legacies)
        {
            try
            {
                log.Info("[INFO] Saving legacy databases.");
                var json = JsonSerializer.Serialize(legacies, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(legacyFile, json);
            }
            catch (Exception ex)
            {
                log.Error($"[ERROR] {ex.Message}");
            }
        }
    }
}
