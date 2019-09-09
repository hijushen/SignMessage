using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace NexChip.SignMessage.Utils
{
    public static class JsonSerializeExt
    {
        public static T DeserializeModel<T>(this T model, string json)
        {
            try
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception)
            {
                return model;
            }
        }

        public static T DeserializeModel<T>(this T model, DataTable dt)
        {
            try
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(Newtonsoft.Json.JsonConvert.SerializeObject(dt));
            }
            catch (Exception)
            {
                return model;
            }
        }


        public static T DeserializeJSON<T>(this string json) where T : new()
        {
            try
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>((json));
            }
            catch (Exception)
            {
                return new T();
            }
        }


        public static string SerializeModel(this object model)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(model);
        }

        public static List<T> NMList<T>(this T model)// where T:new()
        {
            var L = new List<T>();
            return L;
        }

        public static Hashtable NMToHashTable<T>(this T model)
        {
            var ht = new Hashtable();
            foreach (var f in model.GetType().GetProperties())
            {
                ht[f.Name] = f.GetValue(model, new object[] { });
            }
            return ht;
        }
    }
}
