using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace webservice
{
    class Config
    {
        private static string configName = "webservice.exe.config";
        private static string configPath = System.Windows.Forms.Application.StartupPath + "\\" + configName;

        /// <summary>
        /// 获取config配置
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetConfig(string key)
        {
            if (System.IO.File.Exists(configPath))
            {
                ExeConfigurationFileMap ecf = new ExeConfigurationFileMap();
                ecf.ExeConfigFilename = configPath;
                Configuration config = System.Configuration.ConfigurationManager.OpenMappedExeConfiguration(ecf, ConfigurationUserLevel.None);
                var keys = config.AppSettings.Settings.AllKeys.ToList();
                if (keys == null || keys.Count == 0)
                {
                    return null;
                }

                if (keys.Contains(key))
                {
                    key = config.AppSettings.Settings[key].Value.ToString();
                }

            }
            else
            {
                //MessageBox.Show("配置文件不存在，请检查！");
            }
            return key;
        }
        /// <summary>
        /// 保存config配置
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SaveConfig(string key, string value)
        {
            ExeConfigurationFileMap ecf = new ExeConfigurationFileMap();
            ecf.ExeConfigFilename = configPath;
            Configuration config = System.Configuration.ConfigurationManager.OpenMappedExeConfiguration(ecf, ConfigurationUserLevel.None);
            if (config.AppSettings.Settings[key] != null)
            {
                config.AppSettings.Settings[key].Value = value;
            }
            else
            {
                config.AppSettings.Settings.Add(key, value);
            }
            config.Save(ConfigurationSaveMode.Modified);
        }
    }
}
