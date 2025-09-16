using Contracts;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WcfDatabaseServer.Services
{
    public class XmlStorage : IDataStorage
    {
        private readonly string dbFile = "dbs.xml";
        private readonly string legacyFile = "legacies.xml";
        private static readonly ILog log = LogManager.GetLogger(typeof(XmlStorage));

        public List<DataBase> LoadDatabases()
        {
            try
            {
                log.Info("[INFO] Loading databases.");
                if (!File.Exists(dbFile)) return new List<DataBase>();
                using (var fs = new FileStream(dbFile, FileMode.Open))
                {
                    var ser = new XmlSerializer(typeof(List<DataBase>));
                    return (List<DataBase>)ser.Deserialize(fs);
                }
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
                if (!File.Exists(legacyFile)) return new List<LegacyDatabase>();
                using (var fs = new FileStream(legacyFile, FileMode.Open))
                {
                    var ser = new XmlSerializer(typeof(List<LegacyDatabase>));
                    return (List<LegacyDatabase>)ser.Deserialize(fs);
                }
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
                using (var fs = new FileStream(dbFile, FileMode.Create))
                {
                    var ser = new XmlSerializer(typeof(List<DataBase>));
                    ser.Serialize(fs, dbs);
                }
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
                using (var fs = new FileStream(legacyFile, FileMode.Create))
                {
                    var ser = new XmlSerializer(typeof(List<LegacyDatabase>));
                    ser.Serialize(fs, legacies);
                }
            }
            catch (Exception ex)
            {
                log.Error($"[ERROR] {ex.Message}");
            }
        }
    }
}
