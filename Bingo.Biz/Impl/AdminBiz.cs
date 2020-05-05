﻿using Bingo.Biz.Impl.Builder;
using Bingo.Biz.Interface;
using Bingo.Dao.BingoDb.Dao;
using Bingo.Dao.BingoDb.Dao.Impl;
using Bingo.Model.Base;
using Bingo.Model.Contract;
using Infrastructure;
using System.Collections.Generic;

namespace Bingo.Biz.Impl
{
    public class AdminBiz : IAdminBiz
    {
        private readonly IMomentDao momentDao = SingletonProvider<MomentDao>.Instance;
        private readonly IUserInfoBiz uerInfoBiz = SingletonProvider<UserInfoBiz>.Instance;
        private readonly IApplyDetailDao applyDetailDao = SingletonProvider<ApplyDetailDao>.Instance;

        public ResponseContext<MomentCheckDetailType> MomentCheckDetail(RequestContext<MomentCheckDetailRequest> request)
        {
            var moment = momentDao.GetMomentByMomentId(request.Data.MomentId);
            if (moment == null)
            {
                return new ResponseContext<MomentCheckDetailType>(ErrCodeEnum.DataIsnotExist);
            }
            bool overCount = ApplyBuilder.IsOverCount(moment);
            var userInfo = uerInfoBiz.GetUserInfoByUid(moment.UId);
            return new ResponseContext<MomentCheckDetailType>()
            {
                Data = new MomentCheckDetailType()
                {
                    MomentId = moment.MomentId,
                    State = moment.State,
                    StateDesc = MomentContentBuilder.MomentStateMap(moment.State, moment.StopTime, overCount),
                    TextColor = MomentContentBuilder.TextColorMap(moment.State, moment.StopTime, overCount),
                    UserInfo = UserInfoBuilder.BuildUserInfo(userInfo, request.Head),
                    ContentList = MomentContentBuilder.BuilderContent(moment, false),
                    ApplyList = ApplyBuilder.GetApplyList(moment.MomentId, false, request.Head, moment.UId)
                }
            };
        }

        public ResponseContext<MomentCheckDetailResponse> MomentCheckList(RequestContext<MomentCheckList> request)
        {
            var response = new ResponseContext<MomentCheckDetailResponse>()
            {
                Data = new MomentCheckDetailResponse()
                {
                    MomentList = new List<MomentCheckDetailType>()
                }
            };
            var momentList = momentDao.GetMomentListByState(request.Data.State);
            if (momentList.IsNullOrEmpty())
            {
                return response;
            }
            foreach (var moment in momentList)
            {
                var userInfo = uerInfoBiz.GetUserInfoByUid(moment.UId);
                if (userInfo == null)
                {
                    continue;
                }
                var applyList = applyDetailDao.GetListByMomentId(moment.MomentId);
                var dto = new MomentCheckDetailType()
                {
                    MomentId = moment.MomentId,
                    State = moment.State,
                    ShareTitle = moment.Content,
                    ApplyCountDesc = ApplyBuilder.GetServiceCountDesc(applyList,moment.UId),
                    StateDesc = MomentContentBuilder.MomentStateMap(moment.State, moment.StopTime, false),
                    TextColor = MomentContentBuilder.TextColorMap(moment.State, moment.StopTime, false),
                    UserInfo = UserInfoBuilder.BuildUserInfo(userInfo, request.Head),
                    ContentList = MomentContentBuilder.BuilderContent(moment, false)
                };
                response.Data.MomentList.Add(dto);
            }
            return response;
        }

        public ResponseContext<ServiceDetailResponse> ServiceDetail(RequestHead head)
        {
            var response = new ResponseContext<ServiceDetailResponse>()
            {
                Data = new ServiceDetailResponse()
                {
                    PendingCount=momentDao.PendingCount()
                }
            };
            return response;
        }
    }
}