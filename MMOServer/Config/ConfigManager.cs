using IniParser;
using IniParser.Model;
using System;
using System.IO;
using MMOServer.Other;
using MMOServer.ConsoleStuff;

namespace MMOServer.Config
{
    class ConfigManager
    {
        private FileIniDataParser _parser;
        private IniData _data;

        public Settings Settings { get; private set; }

        private const string ConfigFileName = "Config.ini";

        public ConfigManager()
        {
            _parser = new FileIniDataParser();
            Settings = new Settings();
            Load();
        }

        private void Load()
        {
            if (!File.Exists(ConfigFileName))
            {
                Save();
                return;
            }

            try
            {
                _data = _parser.ReadFile(ConfigFileName);

                foreach (var property in Settings.GetType().GetProperties())
                {
                    var type = property.GetType();
                    var value = Convert.ChangeType(_data[((SettingAttribute)property.GetCustomAttributes(typeof(SettingAttribute), false)[0]).Section][property.Name], property.PropertyType);
                    property.SetValue(Settings, value, null);
                }
                ConsoleUtils.Info("Loaded configuration successfully");
            }
            catch(Exception e)
            {
                ConsoleUtils.Error("An error occurred while trying to load server configurations. Please check the Config.ini file");
                Logger.LogError(e.GetBaseException().ToString());
            }
        }

        public void Save()
        {
            if (!File.Exists(ConfigFileName))
                _data = new IniData();

            foreach (var property in Settings.GetType().GetProperties())
            {
                _data[((SettingAttribute)property.GetCustomAttributes(typeof(SettingAttribute), false)[0]).Section][property.Name] = property.GetValue(Settings, null).ToString();
            }
            _parser.WriteFile(ConfigFileName, _data);
        }
    }
}
