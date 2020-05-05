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

        public ResponseContext<LoginResponse> GetLoginInfoByCode(string code, PlatformEnum platform)
        {
            var response = new ResponseContext<LoginResponse>();
            var openId = AppFactory.Factory(platform).GetOpenId(code);
            if (string.IsNullOrEmpty(openId))
            {
                response.ResultCode = ErrCodeEnum.Failure;
                response.ResultMessage = "登录失败,请稍后重试";
                return response;
            }
            var userInfo = GetUserInfoByOpenId(openId);
            if (userInfo == null)
            {
                userInfo = new UserInfoEntity()
                {
                    OpenId = openId,
                    Platform = platform,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now,
                    LastLoginTime = DateTime.Now
                };
                userInfo.UId = userInfoDao.InsertUserInfo(userInfo);
            }
            if (userInfo == null)
            {
                response.ResultCode = ErrCodeEnum.Failure;
                response.ResultMessage = "登录失败,请稍后重试";
                return response;
            }
            response.Data=new LoginResponse()
            {
                UId = userInfo.UId,
                Token = TokenUtil.GenerateToken(userInfo.Platform.ToString(), userInfo.UId)
            };
            return response;
        }

        public ResponseContext<UserInfoType> GetUserInfo(RequestHead head)
        {
            var response = new ResponseContext<UserInfoType>();
            var userInfo = GetUserInfoByUid(head.UId);
            if (userInfo == null)
            {
                response.ResultCode = ErrCodeEnum.UserNoExist;
                response.ResultMessage= ErrCodeEnum.UserNoExist.ToDescription();
                return response;
            }
            response.Data = UserInfoBuilder.BuildUserInfo(userInfo, head,false);
            response.Data.NickName = userInfo.NickName;
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
                response.Data = UserInfoBuilder.BuildUserInfo(userInfo, request.Head,false);
                response.Data.NickName = userInfo.NickName;
            }
            return response;
        }

        public ResponseContext<UpdateUserInfoType> GetUserUpdateInfo(RequestHead head)
        {
            var response = new ResponseContext<UpdateUserInfoType>();
            var userInfo = GetUserInfoByUid(head.UId);
            if (userInfo == null|| userInfo.Platform!=head.Platform)
            {
                response.ResultCode = ErrCodeEnum.UserNoExist;
                response.ResultMessage = "用户不存在";
                return response;
            }
            response.Data = new UpdateUserInfoType()
            {
                Portrait= userInfo.Portrait,
                NickName = userInfo.NickName,
                Gender = userInfo.Gender,
                LiveState = userInfo.LiveState,
                Grade = userInfo.Grade,
                SchoolName = userInfo.SchoolName,
                BirthDate = userInfo.BirthDate.HasValue?userInfo.BirthDate.Value.ToString("yyyy-MM-dd"):"1990-01-01",
                Mobile = userInfo.Mobile,
                WeChatNo = userInfo.WeChatNo,
                QQNo = userInfo.QQNo
            }; 
            return response;
        }

        public Response UpdateUserInfo(RequestContext<UpdateUserInfoType> request)
        {
            Response response = new Response();
            var userInfo = GetUserInfoByUid(request.Head.UId);
            if (userInfo == null || userInfo.Platform != request.Head.Platform)
            {
                response.ResultCode = ErrCodeEnum.UserNoExist;
                response.ResultMessage = "用户不存在";
                return response;
            }
            userInfo = new UserInfoEntity()
            {
                UId=request.Head.UId,
                OpenId= userInfo.OpenId,
                NickName = request.Data.NickName,
                Gender = request.Data.Gender,
                LiveState = request.Data.LiveState,
                Grade = request.Data.Grade,
                SchoolName = request.Data.SchoolName,
                BirthDate = Convert.ToDateTime(request.Data.BirthDate),
                Mobile = request.Data.Mobile,
                WeChatNo = request.Data.WeChatNo,
                QQNo = request.Data.QQNo,
                UpdateTime = DateTime.Now
            };
            bool success = userInfoDao.UpdateUserInfo(userInfo);
            if (!success)
            {
                return new Response(ErrCodeEnum.Failure,"修改失败");
            }
            else
            {
                //刷新缓存
                var userInfoKey = RedisKeyConst.UserInfoByOpenIdAndUIdCacheKey(userInfo.OpenId, userInfo.UId);
                redisClient.Remove(userInfoKey);
                GetUserInfoByUid(request.Head.UId);
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
