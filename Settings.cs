using System;
using System.Configuration;

namespace Launcher
{
    class Settings
    {
        public static string ReadKey(string key)
        {
            if (ConfigurationManager.AppSettings[key] == null)
            {
                return null;
            }
            return ConfigurationManager.AppSettings[key];
        }

        public static void WriteKey(string key, string value)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error writing key");
            }
        }
    }
}
