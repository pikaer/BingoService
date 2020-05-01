using Bingo.Biz.Interface;
using Bingo.Dao.BingoDb.Dao;
using Bingo.Dao.BingoDb.Dao.Impl;
using Bingo.Dao.BingoDb.Entity;
using Bingo.Utils;
using Infrastructure;
using System;

namespace Bingo.Biz.Impl
{
    public class UserInfoBiz : IUserInfoBiz
    {
        private readonly IUserInfoDao userInfoDao = SingletonProvider<UserInfoDao>.Instance;
        private readonly RedisHelper redisClient = RedisHelper.Instance;

        public long GetUIdByCode(string code, PlatformEnum platform)
        {
            var openId = AppFactory.Factory(platform).GetOpenId(code);
            if (string.IsNullOrEmpty(openId))
            {
                return 0;
            }
            var userInfo = GetUserInfoByOpenId(openId);
            if (userInfo != null)
            {
                return userInfo.UId;
            }
            userInfo = new UserInfoEntity()
            {
                OpenId = openId,
                Platform = platform,
                CreateTime=DateTime.Now,
                UpdateTime=DateTime.Now,
                LastLoginTime = DateTime.Now
            };
            return userInfoDao.InsertUserInfo(userInfo);
        }

        public UserInfoEntity GetUserInfoByOpenId(string openId)
        {
            if (string.IsNullOrEmpty(openId))
            {
                return null;
            }
            UserInfoEntity userInfo = null;
            string cacheKey = RedisKeyConst.UserInfoKeyByOpenIdCacheKey(openId);
            string userInfoKey = redisClient.Get(cacheKey);
            if (!string.IsNullOrEmpty(userInfoKey))
            {
                userInfo = redisClient.Get<UserInfoEntity>(userInfoKey);
            }
            if (userInfo != null)
            {
                return userInfo;
            }
            userInfo = userInfoDao.GetUserInfoByOpenId(openId);
            if (userInfo != null)
            {
                if (string.IsNullOrEmpty(userInfoKey))
                {
                    userInfoKey = RedisKeyConst.UserInfoByOpenIdAndUIdCacheKey(userInfo.OpenId, userInfo.UId);
                }
                //缓存一个月
                redisClient.Set(userInfoKey, userInfo, RedisKeyConst.UserInfoCacheSecond);
            }
            return userInfo;
        }


        public UserInfoEntity GetUserInfoByUid(long uid)
        {
            UserInfoEntity userInfo = null;
            string cacheKey = RedisKeyConst.UserInfoKeyByUIdCacheKey(uid);
            string userInfoKey = redisClient.Get(cacheKey);
            if (!string.IsNullOrEmpty(userInfoKey))
            {
                userInfo = redisClient.Get<UserInfoEntity>(userInfoKey);
            }
            if (userInfo != null)
            {
                return userInfo;
            }
            userInfo = userInfoDao.GetUserInfoByUid(uid);
            if (userInfo != null)
            {
                if (string.IsNullOrEmpty(userInfoKey))
                {
                    userInfoKey = RedisKeyConst.UserInfoByOpenIdAndUIdCacheKey(userInfo.OpenId, userInfo.UId);
                }
                //缓存一个月
                redisClient.Set(userInfoKey, userInfo, RedisKeyConst.UserInfoCacheSecond);
            }
            return userInfo;
        }

    }
}
