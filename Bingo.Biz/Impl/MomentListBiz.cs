using Bingo.Biz.Impl.Builder;
using Bingo.Biz.Interface;
using Bingo.Dao.BingoDb.Dao;
using Bingo.Dao.BingoDb.Dao.Impl;
using Bingo.Dao.BingoDb.Entity;
using Bingo.Model.Base;
using Bingo.Model.Common;
using Bingo.Model.Contract;
using Infrastructure;
using System;
using System.Collections.Generic;

namespace Bingo.Biz.Impl
{
    public class MomentListBiz : IMomentListBiz
    {
        private readonly ILogBiz log= SingletonProvider<LogBiz>.Instance;
        private readonly IUserInfoBiz uerInfoBiz = SingletonProvider<UserInfoBiz>.Instance;
        private readonly IMomentDao momentDao = SingletonProvider<MomentDao>.Instance;
        private readonly RedisHelper redisClient = RedisHelper.Instance;

        public ResponseContext<MomentListResponse> MomentList(RequestContext<MomentListRequest> request)
        {
            try
            {
                ResponseContext<MomentListResponse> response = new ResponseContext<MomentListResponse>
                {
                    Data = new MomentListResponse()
                    {
                        MomentList=new List<MomentDetailType>()
                    }
                };
                List<MomentEntity> moments = momentDao.GetMomentListByParam();
                if (moments.IsNullOrEmpty())
                {
                    return response;
                }
                foreach(var moment in moments)
                {
                    var userInfo = uerInfoBiz.GetUserInfoByUid(moment.UId);
                    if (userInfo == null)
                    {
                        continue;
                    }
                    var dto = new MomentDetailType()
                    {
                        UserInfo = UserInfoBuilder.BuildUserInfo(userInfo),
                        ContentList= MomentContentBuilder.BuilderContent(moment)
                    };
                    response.Data.MomentList.Add(dto);
                }
                return response;
            }
            catch(Exception ex)
            {
                log.ErrorAsync("MomentListBiz.MomentList", ex);
                return null;
            }
        }

    }
}
