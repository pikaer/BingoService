using Bingo.Biz.Impl.Builder;
using Bingo.Biz.Interface;
using Bingo.Dao.BingoDb.Dao;
using Bingo.Dao.BingoDb.Dao.Impl;
using Bingo.Dao.BingoDb.Entity;
using Bingo.Model.Base;
using Bingo.Model.Common;
using Bingo.Model.Contract;
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

        public ResponseContext<UserInfoType> GetUserInfo(long uid)
        {
            var response = new ResponseContext<UserInfoType>();
            var userInfo = GetUserInfoByUid(uid);
            if (userInfo == null)
            {
                response.ResultCode = ErrCodeEnum.UserNoExist;
                response.ResultMessage= ErrCodeEnum.UserNoExist.ToDescription();
                return response;
            }
            response.Data = UserInfoBuilder.BuildUserInfo(userInfo);
            return response;
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
                    redisClient.Set(cacheKey, userInfoKey, RedisKeyConst.UserInfoCacheSecond);
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
                    redisClient.Set(cacheKey, userInfoKey, RedisKeyConst.UserInfoCacheSecond);
                }
                //缓存一个月
                redisClient.Set(userInfoKey, userInfo, RedisKeyConst.UserInfoCacheSecond);
            }
            return userInfo;
        }

        public ResponseContext<UserInfoType> Register(RequestContext<RegisterRequest> request)
        {
            var response = new ResponseContext<UserInfoType>();
            var userInfo = GetUserInfoByUid(request.Head.UId);
            if (userInfo == null)
            {
                response.ResultCode = ErrCodeEnum.UserNoExist;
                response.ResultMessage = "请求服务器失败，请重试！";
                return response;
            }
            bool success = userInfoDao.Register(request.Head.UId, request.Data.Gender, request.Data.NickName, request.Data.AvatarUrl, request.Data.Country, request.Data.Province, request.Data.City);
            if (success)
            {
                //刷新缓存
                var userInfoKey = RedisKeyConst.UserInfoByOpenIdAndUIdCacheKey(userInfo.OpenId, userInfo.UId);
                redisClient.Remove(userInfoKey);
                userInfo=GetUserInfoByUid(request.Head.UId);
                response.Data = UserInfoBuilder.BuildUserInfo(userInfo);
            }
            return response;
        }

        public bool UpdateUserLocation(long uId, double latitude, double longitude)
        {
            bool success = userInfoDao.UpdateUserLocation(uId, latitude, longitude);
            if (success)
            {
                var userInfo = GetUserInfoByUid(uId);
                if (userInfo != null)
                {
                    //刷新缓存
                    var userInfoKey = RedisKeyConst.UserInfoByOpenIdAndUIdCacheKey(userInfo.OpenId, userInfo.UId);
                    redisClient.Remove(userInfoKey);
                    GetUserInfoByUid(uId);
                }
            }
            return success;
        }

    }
}
