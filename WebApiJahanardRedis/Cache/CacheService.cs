using Newtonsoft.Json;
using StackExchange.Redis;

namespace WebApiJahanardRedis.Cache
{
    public class CacheService : ICacheServiceJahangard
    {
        ////////////ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("127.0.0.1:6379,abortConnect=false,connectTimeout=30000,responseTimeout=30000,resolvedns=1,endpoint");
        private IDatabase _db;

        public CacheService()
        {
            ConfigureRedis();
        }

        private void ConfigureRedis()
        {
            try
            {
                _db = ConnectionHelper.Connection.GetDatabase();
                //////////////_db = redis.GetDatabase();
            }
            catch (Exception exx)
            {

                string s = exx.Message.ToString();
            }

        }

        public T GetData<T>(string key)
        {
            var value = _db.StringGet(key);
            if (!string.IsNullOrEmpty(value))
            {
                return JsonConvert.DeserializeObject<T>(value);
            }
            return default;
        }

        public bool SetData<T>(string key, T value, DateTimeOffset expirationTime)
        {
            TimeSpan expiryTime = expirationTime.DateTime.Subtract(DateTime.Now);
            var isSet = _db.StringSet(key, JsonConvert.SerializeObject(value), expiryTime);
            return isSet;
        }
        public object RemoveData(string key)
        {
            bool _isKeyExist = false;
            try
            {
                _isKeyExist = _db.KeyExists(key);
                if (_isKeyExist == true)
                {
                    return _db.KeyDelete(key);
                }
                
            }
            catch (Exception ex)
            {

                string s = ex.Message.ToString();
            }


            return _isKeyExist;
        }

        
    }
}
