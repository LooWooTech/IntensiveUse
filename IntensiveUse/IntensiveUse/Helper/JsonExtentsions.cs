using Newtonsoft.Json;

namespace IntensiveUse.Helper
{
    public static class JsonExtentsions
    {
        public static string ToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static T ToObject<T>(this string jsonString)
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }
    }
}