using System.Text;
using Newtonsoft.Json;

namespace Common
{
    public static class ObjectSerialize
    {
        public static byte[] Serialize(this object obj)
        {
            if (obj == null)
            {
                return null;
            }

            var json = JsonConvert.SerializeObject(obj);
            return Encoding.ASCII.GetBytes(json);
        }

        public static T Deserialize<T>(this byte[] arrBytes)
        {
            var json = Encoding.Default.GetString(arrBytes);
            return JsonConvert.DeserializeObject<T>(json);
        }
        public static object Deserialize(this byte[] arrBytes)
        {
            return Encoding.Default.GetString(arrBytes);
        }
    }
}