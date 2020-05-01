namespace Bingo.Utils
{
    public class RedisKeyConst
    {
        public static readonly int UserInfoCacheSecond = 2592000; //缓存时间一个月
        private static readonly string UserInfoKeyByOpenIdKey = "UserInfoKeyByOpenIdKey_{0}";
        private static readonly string UserInfoKeyByUIdKey = "UserInfoKeyByUIdKey_{0}";
        private static readonly string UserInfoByOpenIdAndUIdKey = "UserInfoByOpenIdAndUIdKey_{0}_{1}";

        /// <summary>
        /// 通过openId 获取缓存key
        /// </summary>
        public static string UserInfoKeyByOpenIdCacheKey(string openId)
        {
            return string.Format(UserInfoKeyByOpenIdKey, openId);
        }

        /// <summary>
        /// 通过UId 获取缓存key
        /// </summary>
        public static string UserInfoKeyByUIdCacheKey(long uid)
        {
            return string.Format(UserInfoKeyByUIdKey, uid);
        }

        /// <summary>
        /// 通过openId和Uid 获取缓存key
        /// </summary>
        public static string UserInfoByOpenIdAndUIdCacheKey(string openId,long uid)
        {
            return string.Format(UserInfoByOpenIdAndUIdKey, uid, openId);
        }
    }
}
