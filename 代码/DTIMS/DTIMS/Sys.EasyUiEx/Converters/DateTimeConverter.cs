using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json.Converters;

namespace Sys.EasyUiEx.Converters
{
    /// <summary>
    /// 自定义 DateTime JSON 转换器。
    /// </summary>
    internal class DateTimeConverter : DateTimeConverterBase
    {
        public override object ReadJson(Newtonsoft.Json.JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            return DateTime.Parse(reader.ToString());
        }

        public override void WriteJson(Newtonsoft.Json.JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }
    }
}
