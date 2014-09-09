using System;
using System.Configuration;
using Memcached.Client;

namespace Skpic.Common
{
    /// <summary>
    /// 缓存服务器帮助类
    /// </summary>
    public class MemcachedHelper
    {
        /// <summary>
        /// memcached客户端
        /// </summary>
        private static MemcachedClient _mc;

        /// <summary>
        /// memcached缓存服务器构造函数
        /// </summary>
        static MemcachedHelper()
        {
            Init();
        }

        /// <summary>
        /// 初始化memcached
        /// </summary>
        private static void Init()
        {
            //分布Memcachedf服务IP 端口
            //组成了memcached集群。
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
        /// 当 key 不存在的時候才保存 value
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns>是否添加成功</returns>
        public static bool Add(string key, object value)
        {
            return _mc.Add(key, value);
        }

        /// <summary>
        /// 当 key 不存在的時候才保存 value
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expiry">过期时间，以秒为单位，0 表示永远</param>
        /// <returns>是否添加成功</returns>
        public static bool Add(string key, object value, DateTime expiry)
        {
            return _mc.Add(key, value, expiry);
        }

        /// <summary>
        /// 当 key 不存在的時候才保存 value
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expiry">过期时间，以秒为单位，0 表示永远</param>
        /// <param name="hashCode"></param>
        /// <returns>是否添加成功</returns>
        public static bool Add(string key, object value, DateTime expiry, int hashCode)
        {
            return _mc.Add(key, value, expiry, hashCode);
        }

        /// <summary>
        /// 当 key 不存在的時候才保存 value
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="hashCode"></param>
        /// <returns>是否添加成功</returns>
        public static bool Add(string key, object value, int hashCode)
        {
            return _mc.Add(key, value, hashCode);
        }

        #endregion

        #region Set

        /// <summary>
        /// 直接写入新的value，如果key存在就是替换value
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns>是否添加成功</returns>
        public static bool Set(string key, string value)
        {
            return _mc.Set(key, value);
        }
        /// <summary>
        /// 直接写入新的value，如果key存在就是替换value
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expiry">过期时间,以秒为单位，0 表示永远</param>
        /// <returns>是否添加成功</returns>
        public static bool Set(string key, string value, DateTime expiry)
        {
            return _mc.Set(key, value, expiry);
        }

        /// <summary>
        /// 直接写入新的value，如果key存在就是替换value
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expiry">过期时间,以秒为单位，0 表示永远</param>
        /// <param name="hashCode">指定hashCode标志</param>
        /// <returns>是否添加成功</returns>
        public static bool Set(string key, object value, DateTime expiry, int hashCode)
        {
            return _mc.Set(key, value);
        }

        /// <summary>
        /// 直接写入新的value，如果key存在就是替换value
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="hashCode">指定hashCode标志</param>
        /// <returns>是否添加成功</returns>
        public static bool Set(string key, object value, int hashCode)
        {
            return _mc.Set(key, value, hashCode);
        }

        #endregion

        #region Replace

        /// <summary>
        /// 当key相同的時候替换value
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns>是否替换成功</returns>
        public static bool Replace(string key, string value)
        {
            return _mc.Replace(key, value);
        }
        /// <summary>
        /// 当key相同的時候替换value
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expiry">过期时间,以秒为单位，0 表示永远</param>
        /// <returns>是否替换成功</returns>
        public static bool Replace(string key, string value, DateTime expiry)
        {
            return _mc.Replace(key, value, expiry);
        }

        /// <summary>
        /// 当key相同的時候替换value
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expiry">过期时间,以秒为单位，0 表示永远</param>
        /// <param name="hashCode">指定hashCode标志</param>
        /// <returns>是否替换成功</returns>
        public static bool Replace(string key, object value, DateTime expiry, int hashCode)
        {
            return _mc.Replace(key, value);
        }

        /// <summary>
        /// 当key相同的時候替换value
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="hashCode">指定hashCode标志</param>
        /// <returns>是否替换成功</returns>
        public static bool Replace(string key, object value, int hashCode)
        {
            return _mc.Replace(key, value, hashCode);
        }

        #endregion

        #region Get

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>key对应的value</returns>
        public static object Get(string key)
        {
            return _mc.Get(key);
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="hashCode">指定hashCode</param>
        /// <param name="asString">是否直接返回字符串</param>
        /// <returns>key对应的value</returns>
        public static object Get(string key, int hashCode, bool asString)
        {
            return _mc.Get(key, hashCode, asString);
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="hashCode">指定hashCode</param>
        /// <returns>key对应的value</returns>
        public static object Get(string key, int hashCode)
        {
            return _mc.Get(key, hashCode);
        }

        #endregion

        #region Delete

        /// <summary>
        /// 删除指定key的value
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>是否删除成功</returns>
        public static bool Delete(string key)
        {
            return _mc.Delete(key);
        }

        /// <summary>
        /// 删除指定key的value
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="expiry">过期时间,以秒为单位，0 表示永远</param>
        /// <returns>是否删除成功</returns>
        public static bool Delete(string key, DateTime expiry)
        {
            return _mc.Delete(key, expiry);
        }

        /// <summary>
        /// 删除指定key的value
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="hashCode">指定hashCode</param>
        /// <param name="expiry">过期时间,以秒为单位，0 表示永远</param>
        /// <returns>是否删除成功</returns>
        public static bool Delete(string key, object hashCode, DateTime expiry)
        {
            return _mc.Delete(key, hashCode, expiry);
        }

        #endregion
    }
}
