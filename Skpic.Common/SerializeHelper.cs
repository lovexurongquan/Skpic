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
    /// 序列化帮助类
    /// </summary>
    public static class SerializeHelper
    {
        /// <summary>
        /// 序列化对象为string字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string SerializeObjectToJson<T>(this T value)
        {
            return JsonConvert.SerializeObject(value, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        }

        /// <summary>
        /// 反序列化字符串为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T DeSerializeStringToObject<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}