using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using BJ.Sys.EasyUiEx.Converters;

namespace BJ.Sys.EasyUiEx.Helper
{
    /// <summary>
    /// JSON序列化、反序列化帮助类。
    /// </summary>
    public class JsonHelper
    {
        /// <summary>
        /// JSON序列化，将对象序列化为JSON字符串。
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string JsonSerializer(object obj)
        {
            string jsonString = JsonConvert.SerializeObject(obj, new DateTimeConverter());
            return jsonString;
        }

        /// <summary>
        /// JSON序列化，将对象序列化为JSON字符串。
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static string JsonSerializer(DataTable table)
        {
            string jsonString = JsonConvert.SerializeObject(table, new DataTableConverter(), new DateTimeConverter());
            return jsonString;
        }

        /// <summary>
        /// JSON反序列化，将JSON字符串反序列化为对象。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="jsonString">反序列化为对象的JSON字符串。</param>
        /// <returns></returns>
        public static T JsonDeserialize<T>(string jsonString)
        {
            T desT = JsonConvert.DeserializeObject<T>(jsonString, new DateTimeConverter());
            return desT;
        }

        /// <summary>
        /// JSON反序列化，将JSON字符串反序列化为对象。
        /// </summary>
        /// <param name="jsonString">反序列化为对象的JSON字符串。</param>
        /// <returns></returns>
        public static DataTable JsonDeserialize(string jsonString)
        {
            DataTable desT = JsonConvert.DeserializeObject<DataTable>(jsonString, new DataTableConverter(), new DateTimeConverter());
            return desT;
        }
    }//end public class JsonHelper
}
