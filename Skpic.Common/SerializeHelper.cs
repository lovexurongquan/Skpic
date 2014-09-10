/*
 * Added by laoxu 2014-09-10 11:00:00
 * ---------------------------------------------------------------
 * for：string extend class. serialize model class.
 * ---------------------------------------------------------------
 * version:1.0
 * mail:lovexurongquan@163.com
 */

using Newtonsoft.Json;

namespace Skpic.Common
{
    /// <summary>
    /// serialize helper class.
    /// </summary>
    public static class SerializeHelper
    {
        /// <summary>
        /// serialize object to json string.
        /// </summary>
        /// <typeparam name="T">entity type.</typeparam>
        /// <param name="value">object value.</param>
        /// <returns>json string</returns>
        public static string SerializeObjectToJson<T>(this T value)
        {
            return JsonConvert.SerializeObject(value, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        }

        /// <summary>
        /// deserialize string to object.
        /// </summary>
        /// <typeparam name="T">entity type.</typeparam>
        /// <param name="json">json string.</param>
        /// <returns>entyty.</returns>
        public static T DeSerializeStringToObject<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}