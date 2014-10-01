/*
 * Added by laoxu 2014-09-10 11:00:00
 * ---------------------------------------------------------------
 * for：connect memcached class.
 * ---------------------------------------------------------------
 * version:1.0
 * mail:lovexurongquan@163.com
 */

using System;
using System.Configuration;
using Memcached.Client;

namespace Skpic.Common
{
    /// <summary>
    /// Help class cache servers
    /// </summary>
    public class MemcachedHelper
    {
        /// <summary>
        /// memcached client
        /// </summary>
        private static MemcachedClient _mc;

        /// <summary>
        /// memcached server Constructor
        /// </summary>
        static MemcachedHelper()
        {
            Init();
        }

        /// <summary>
        /// Initialization memcached
        /// </summary>
        private static void Init()
        {
            //string[] servers = { "192.168.1.100:11211", "192.168.1.15:11211" };
            //<add key="memcached" value="192.168.1.100:11211,192.168.1.15:11211"/>
            string[] services = ConfigurationManager.AppSettings["memcached"].Split(',');


            SockIOPool pool = SockIOPool.GetInstance("test");
            pool.SetServers(services);
            pool.InitConnections = 3;
            pool.MinConnections = 3;
            pool.MaxConnections = 5;
            pool.SocketConnectTimeout = 1000;
            pool.SocketTimeout = 3000;
            pool.MaintenanceSleep = 30;

            pool.Failover = true;
            pool.Nagle = false;

            pool.Initialize();
            _mc = new MemcachedClient { EnableCompression = false, PoolName = "test" };
        }

        #region Add

        /// <summary>
        /// When the key does not exist only to save the value.
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        /// <returns>Whether to add success</returns>
        public static bool Add(string key, object value)
        {
            return _mc.Add(key, value);
        }

        /// <summary>
        /// When the key does not exist only to save the value.
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        /// <param name="expiry">Expiration time , in seconds , 0 means forever</param>
        /// <returns>Whether to add success</returns>
        public static bool Add(string key, object value, DateTime expiry)
        {
            return _mc.Add(key, value, expiry);
        }

        /// <summary>
        /// When the key does not exist only to save the value.
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        /// <param name="expiry">Expiration time , in seconds , 0 means forever</param>
        /// <param name="hashCode"></param>
        /// <returns>Whether to add success</returns>
        public static bool Add(string key, object value, DateTime expiry, int hashCode)
        {
            return _mc.Add(key, value, expiry, hashCode);
        }

        /// <summary>
        /// When the key does not exist only to save the value.
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        /// <param name="hashCode"></param>
        /// <returns>Whether to add success</returns>
        public static bool Add(string key, object value, int hashCode)
        {
            return _mc.Add(key, value, hashCode);
        }

        #endregion

        #region Set

        /// <summary>
        /// Direct write the new value, if the key is to replace the value exists.
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        /// <returns>Whether to add success</returns>
        public static bool Set(string key, string value)
        {
            return _mc.Set(key, value);
        }
        /// <summary>
        /// Direct write the new value, if the key is to replace the value exists.
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        /// <param name="expiry">Expiration time , in seconds , 0 means forever</param>
        /// <returns>Whether to add success</returns>
        public static bool Set(string key, string value, DateTime expiry)
        {
            return _mc.Set(key, value, expiry);
        }

        /// <summary>
        /// Direct write the new value, if the key is to replace the value exists.
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        /// <param name="expiry">Expiration time , in seconds , 0 means forever</param>
        /// <param name="hashCode">Specify hashCode flag</param>
        /// <returns>Whether to add success</returns>
        public static bool Set(string key, object value, DateTime expiry, int hashCode)
        {
            return _mc.Set(key, value);
        }

        /// <summary>
        /// Direct write the new value, if the key is to replace the value exists.
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        /// <param name="hashCode">Specify hashCode flag</param>
        /// <returns>Whether to add success</returns>
        public static bool Set(string key, object value, int hashCode)
        {
            return _mc.Set(key, value, hashCode);
        }

        #endregion

        #region Replace

        /// <summary>
        /// When the key the same time replace the value.
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        /// <returns>Whether to replace the successful</returns>
        public static bool Replace(string key, string value)
        {
            return _mc.Replace(key, value);
        }
        /// <summary>
        /// When the key the same time replace the value.
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        /// <param name="expiry">Expiration time , in seconds , 0 means forever.</param>
        /// <returns>Whether to replace the successful</returns>
        public static bool Replace(string key, string value, DateTime expiry)
        {
            return _mc.Replace(key, value, expiry);
        }

        /// <summary>
        /// When the key the same time replace the value.
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        /// <param name="expiry">Expiration time , in seconds , 0 means forever</param>
        /// <param name="hashCode">Specify hashCode flag</param>
        /// <returns>Whether to replace the successful</returns>
        public static bool Replace(string key, object value, DateTime expiry, int hashCode)
        {
            return _mc.Replace(key, value);
        }

        /// <summary>
        /// When the key the same time replace the value.
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        /// <param name="hashCode">Specify hashCode flag</param>
        /// <returns>Whether to replace the successful</returns>
        public static bool Replace(string key, object value, int hashCode)
        {
            return _mc.Replace(key, value, hashCode);
        }

        #endregion

        #region Get

        /// <summary>
        /// Read data.
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>key corresponding value</returns>
        public static object Get(string key)
        {
            return _mc.Get(key);
        }

        /// <summary>
        /// Read data.
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="hashCode">Specify hashCode</param>
        /// <param name="asString">Whether direct return string</param>
        /// <returns>key corresponding value</returns>
        public static object Get(string key, int hashCode, bool asString)
        {
            return _mc.Get(key, hashCode, asString);
        }

        /// <summary>
        /// Read data.
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="hashCode">Specify hashCode</param>
        /// <returns>key corresponding value</returns>
        public static object Get(string key, int hashCode)
        {
            return _mc.Get(key, hashCode);
        }

        #endregion

        #region Delete

        /// <summary>
        /// Delete the specified key value.
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>Whether deleted successfully</returns>
        public static bool Delete(string key)
        {
            return _mc.Delete(key);
        }

        /// <summary>
        /// Delete the specified key value.
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="expiry">Expiration time , in seconds , 0 means forever</param>
        /// <returns>Whether deleted successfully</returns>
        public static bool Delete(string key, DateTime expiry)
        {
            return _mc.Delete(key, expiry);
        }

        /// <summary>
        /// Delete the specified key value.
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="hashCode">Specify hashCode</param>
        /// <param name="expiry">Expiration time , in seconds , 0 means forever</param>
        /// <returns>Whether deleted successfully</returns>
        public static bool Delete(string key, object hashCode, DateTime expiry)
        {
            return _mc.Delete(key, hashCode, expiry);
        }

        #endregion
    }
}
