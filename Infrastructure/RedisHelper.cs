using StackExchange.Redis;
using System;

namespace Infrastructure
{
    public class RedisHelper
    {
        private static readonly object Locker = new object();

        private ConnectionMultiplexer redisMultiplexer;

        IDatabase db = null;

        private static RedisHelper redisClient = null;

        public static RedisHelper Instance
        {
            get
            {
                if (redisClient == null)
                {
                    lock (Locker)
                    {
                        if (redisClient == null)
                        {
                            redisClient = new RedisHelper();
                        }
                    }
                }
                return redisClient;
            }
        }

        public void InitConnect()
        {
            try
            {
                var connection = JsonSettingHelper.AppSettings["RedisConnectionString"];
                redisMultiplexer = ConnectionMultiplexer.Connect(connection);
                db = redisMultiplexer.GetDatabase();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                redisMultiplexer = null;
                db = null;
            }
        }

        public bool Set(string key, string value, int expirySecond)
        {
            try
            {
                if (db == null)
                {
                    return false;
                }
                TimeSpan expiry = TimeSpan.FromSeconds(expirySecond);
                return db.StringSet(key, value, expiry);
            }
            catch
            {
                return false;
            }
        }

        public string Get(string key)
        {
            try
            {
                if (db == null)
                {
                    return default;
                }
                return db.StringGet(key);
            }
            catch
            {
                return default;
            }
            
        }

        public T Get<T>(string key)
        {
            try
            {
                if (db == null)
                {
                    return default;
                }
                var value = db.StringGet(key);
                if (value.IsNullOrEmpty)
                {
                    return default;
                }
                return ObjectHelper.JsonToObject<T>(value);
            }
            catch
            {
                return default;
            }
        }

        public bool Set<T>(string key, T obj, int expirySecond)
        {
            try
            {
                if (db == null)
                {
                    return false;
                }
                TimeSpan expiry = TimeSpan.FromSeconds(expirySecond);
                string json = ObjectHelper.SerializeToString(obj);
                return db.StringSet(key, json, expiry);
            }
            catch
            {
                return false;
            }
            
        }

        public bool Remove(string key)
        {
            try
            {
                if (db == null)
                {
                    return false;
                }
                return db.KeyDelete(key);
            }
            catch
            {
                return false;
            }
        }
    }

}
