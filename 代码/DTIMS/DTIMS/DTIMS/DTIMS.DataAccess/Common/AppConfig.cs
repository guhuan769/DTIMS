using System;

namespace Inphase.Common
{
    /// <summary>
    /// AppConfig ��ժҪ˵����
    /// </summary>
    public class AppConfig
    {
        public AppConfig()
        {
            //
            // TODO: �ڴ˴���ӹ��캯���߼�
            //
        }

        public static string ReadConfigItem(string name)
        {
            //return System.Configuration.ConfigurationSettings.AppSettings[name].ToString();
            return System.Configuration.ConfigurationManager.AppSettings[name];
        }
    }
}
