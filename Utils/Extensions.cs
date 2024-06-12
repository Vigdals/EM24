using Newtonsoft.Json;

namespace MatchBetting.Utils
{
    public static class Extensions
    {
        /// <summary>
        /// Dumps the object as a json string
        /// Can be used for logging object contents.
        /// </summary>
        /// <typeparam name="T">Type of the object.</typeparam>
        /// <param name="obj">The object to dump. Can be null</param>
        /// <param name="indent">To indent the result or not</param>
        /// <returns>the a string representing the object content</returns>
        public static string Dump<T>(this T obj, bool indent = false)
        {
            return JsonConvert.SerializeObject(obj, indent ? Formatting.Indented : Formatting.None,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }
    }
}