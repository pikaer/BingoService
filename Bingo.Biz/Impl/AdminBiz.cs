using Bingo.Biz.Impl.Builder;
using Bingo.Biz.Interface;
using Bingo.Dao.BingoDb.Dao;
using Bingo.Dao.BingoDb.Dao.Impl;
using Bingo.Dao.BingoDb.Entity;
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
                    ApplyList = ApplyBuilder.GetCheckDetails(moment, userInfo, request.Head)
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
                    Address = moment.Address,
                    Latitude = moment.Latitude,
                    Longitude = moment.Longitude,
                    ApplyCountDesc = ApplyBuilder.GetServiceCountDesc(applyList,moment.UId),
                    StateDesc = moment.State== MomentStateEnum.审核中?"待审核":"已审核通过",
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
            int pendingCount = momentDao.PendingCount();
            var response = new ResponseContext<ServiceDetailResponse>()
            {
                Data = new ServiceDetailResponse()
                {
                    Remark = string.Format("待审核总数：{0}",pendingCount)
                }
            };
            return response;
        }
    }
}
