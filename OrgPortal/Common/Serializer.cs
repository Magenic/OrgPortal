using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace OrgPortal.Common
{
    // This class is mostly borrowed from Jerry Nixon on Stack Overflow
    // http://stackoverflow.com/a/10965830
    public static class Serializer
    {
        public static T Deserialize<T>(string json)
        {
            var bytes = Encoding.Unicode.GetBytes(System.Net.WebUtility.UrlDecode(json));
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                var serializer = new DataContractJsonSerializer(typeof(T));
                return (T)serializer.ReadObject(stream);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        public static string Serialize(object instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            using (MemoryStream stream = new MemoryStream())
            {
                var serializer = new DataContractJsonSerializer(instance.GetType());
                serializer.WriteObject(stream, instance);
                stream.Position = 0;
                using (StreamReader reader = new StreamReader(stream))
                {
                    return System.Net.WebUtility.UrlEncode(reader.ReadToEnd());
                }
            }
        }
    }
}
