using Basement.Framework.Common;
using Basement.Framework.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Basement.Framework.Configuration
{
    public static class DatabaseConfig
    {
        private static readonly Hashtable DatabaseInfos = new Hashtable();
        private static readonly DatabaseSection _databaseSectionHandler;

        static DatabaseConfig()
        {
            string databaseConfig = GlobleConfig.ConfigFilesPath + @"\Database.config";
            var fileMap = new ExeConfigurationFileMap { ExeConfigFilename = databaseConfig};
            var configuration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            try
            {
                _databaseSectionHandler = (DatabaseSection)configuration.GetSection("database");
                var databaseKeyType = typeof(EnumDatabase);
                foreach (var key in Enum.GetNames(databaseKeyType))
                {
                    InitDatabaseConfig((EnumDatabase)Enum.Parse(databaseKeyType, key));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                throw;
            }

        }

        public static void InitDatabaseConfig(EnumDatabase key)
        {
            var cfg = GetSetting(key.ToString());
            if (cfg == null)
                throw new Exception(key + "的配置项未找到");
            DatabaseInfo info = new DatabaseInfo
            {
                Server = cfg.Server,
                DatabaseName = cfg.DbName,
                Username = cfg.UserName,
                Password = cfg.Password
            };
            DatabaseInfos.Add(key, info);
        }

        public static SettingElement GetSetting(string name)
        {
            if (_databaseSectionHandler == null || _databaseSectionHandler.Settings == null)
                return null;
            var sitting = _databaseSectionHandler.Settings.Cast<SettingElement>().FirstOrDefault(setting => setting.Name == name);
            if (sitting == null)
            {
                throw new Exception(name + "的配置项未找到");
            }
            return sitting;
        }

        public static DatabaseInfo GetDatabaseInfo(EnumDatabase key)
        {
            if (DatabaseInfos.ContainsKey(key))
            {
                return (DatabaseInfo)DatabaseInfos[key];
            }
            return null;
        }

    }

    public class DatabaseInfo
    {
        public EnumDatabase Key { set; get; }
        public string Server { get; set; }
        public string DatabaseName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public enum EnumDatabase
    {
        NHibernateDB = 0,
        Gift163DB = 1,
        abc = 2,
        cba = 3
    }

}
