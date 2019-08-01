using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace NexChip.SignMessage.Entities
{
    /// <summary>
    /// 日期
    /// </summary>
    public class DateFormatDef : IsoDateTimeConverter
    {
        /// <summary>
        /// 
        /// </summary>
        public DateFormatDef()
        {
            base.DateTimeFormat = "yyyy-MM-dd";
        }
    }

    /// <summary>
    /// 时间
    /// </summary>
    public class DateTimeFormatDef : IsoDateTimeConverter
    {
        /// <summary>
        /// 
        /// </summary>
        public DateTimeFormatDef()
        {
            base.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        }
    }

    public class CustomDateConverter : DateTimeConverterBase
    {
        private IsoDateTimeConverter dtConverter;
        public CustomDateConverter()
        {
            dtConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };
        }
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (!string.IsNullOrEmpty(reader.Value.ToString()))
            {
                return dtConverter.ReadJson(reader, objectType, existingValue, serializer);
            }
            else
            {
                return DateTime.MinValue;
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if ((DateTime)value == DateTime.MinValue)
            {
                new StringEnumConverter().WriteJson(writer, null, serializer);
            }
            else
            {
                dtConverter.WriteJson(writer, value, serializer);
            }
        }
    }

}
