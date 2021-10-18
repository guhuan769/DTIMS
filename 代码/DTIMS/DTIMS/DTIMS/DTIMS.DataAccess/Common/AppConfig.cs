using System;

namespace Inphase.Common
{
    /// <summary>
    /// AppConfig 的摘要说明。
    /// </summary>
    public class AppConfig
    {
        public AppConfig()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        public static string ReadConfigItem(string name)
        {
            //return System.Configuration.ConfigurationSettings.AppSettings[name].ToString();
            return System.Configuration.ConfigurationManager.AppSettings[name];
        }
    }
}
