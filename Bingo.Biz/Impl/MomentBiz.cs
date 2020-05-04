using Bingo.Biz.Impl.Builder;
using Bingo.Biz.Interface;
using Bingo.Dao.BingoDb.Dao;
using Bingo.Dao.BingoDb.Dao.Impl;
using Bingo.Dao.BingoDb.Entity;
using Bingo.Model.Base;
using Bingo.Model.Contract;
using Infrastructure;
using System;
using System.Collections.Generic;

namespace Bingo.Biz.Impl
{
    public class MomentBiz : IMomentBiz
    {
        private readonly IMomentDao momentDao = SingletonProvider<MomentDao>.Instance;
        private readonly IUserInfoBiz uerInfoBiz = SingletonProvider<UserInfoBiz>.Instance;
        private readonly static IApplyInfoDao applyInfoDao = SingletonProvider<ApplyInfoDao>.Instance;

        public Response MomentAction(Guid momentId,string action)
        {
            var moment = MomentBuilder.GetMoment(momentId);
            if (moment == null)
            {
                return new Response(ErrCodeEnum.DataIsnotExist);
            }
            string message = "操作成功";
            switch (action)
            {
                case "stop":
                    momentDao.UpdateStopTime(momentId);
                    message = "已停止活动";
                    break;
                case "delete":
                    momentDao.Delete(momentId);
                    message = "删除成功";
                    break;
                default:
                     break;
            }
            return new Response(ErrCodeEnum.Success, message);
        }

        public ResponseContext<MyPublishListResponse> MyPublishList(RequestContext<MyPublishListRequest> request)
        {
            var response = new ResponseContext<MyPublishListResponse>()
            {
                Data = new MyPublishListResponse()
                {
                    MomentList = new List<MyPublishMomentDetailType>()
                }
            };
            var userInfo = uerInfoBiz.GetUserInfoByUid(request.Head.UId);
            var momentList = momentDao.GetMomentListByUid(request.Head.UId);
            if (momentList.IsNullOrEmpty())
            {
                return response;
            }
            foreach (var moment in momentList)
            {
                bool overCount = ApplyBuilder.IsOverCount(moment);
                var applyList = applyInfoDao.GetListByMomentId(moment.MomentId);
                var dto = new MyPublishMomentDetailType()
                {
                    MomentId = moment.MomentId,
                    State = moment.State,
                    ApplyCountDesc=ApplyBuilder.GetApplyCountDesc(applyList),
                    ApplyCountColor= ApplyBuilder.GetApplyCountColor(applyList),
                    IsOverTime = MomentContentBuilder.IsOverTime(moment.StopTime),
                    ShareFlag = moment.State== MomentStateEnum.正常发布中,
                    StateDesc = MomentContentBuilder.MomentStateMap(moment.State, moment.StopTime, overCount),
                    TextColor= MomentContentBuilder.TextColorMap(moment.State, moment.StopTime, overCount),
                    UserInfo = UserInfoBuilder.BuildUserInfo(userInfo, request.Head),
                    ContentList = MomentContentBuilder.BuilderContent(moment,false)
                };
                response.Data.MomentList.Add(dto);
            }
            return response;
        }

        public ResponseContext<MyPublishMomentDetailType> MyPublishMomentDetail(Guid momentId, RequestHead head)
        {
            var moment = momentDao.GetMomentByMomentId(momentId);
            if (moment == null)
            {
                return new ResponseContext<MyPublishMomentDetailType>(ErrCodeEnum.DataIsnotExist);
            }
            bool overCount = ApplyBuilder.IsOverCount(moment);
            var userInfo = uerInfoBiz.GetUserInfoByUid(moment.UId);
            return new ResponseContext<MyPublishMomentDetailType>()
            {
                Data = new MyPublishMomentDetailType()
                {
                    MomentId = momentId,
                    State = moment.State,
                    IsOverTime = MomentContentBuilder.IsOverTime(moment.StopTime),
                    ShareFlag = moment.State == MomentStateEnum.正常发布中,
                    StateDesc = MomentContentBuilder.MomentStateMap(moment.State,moment.StopTime, overCount),
                    TextColor = MomentContentBuilder.TextColorMap(moment.State, moment.StopTime, overCount),
                    UserInfo = UserInfoBuilder.BuildUserInfo(userInfo, head),
                    ContentList = MomentContentBuilder.BuilderContent(moment,false),
                    ApplyList= ApplyBuilder.GetApplyList(momentId,false, head,moment.UId)
                }
            };
        }

        public Response Publish(RequestContext<PublishMomentRequest> request)
        {
            bool msgSec = AppFactory.Factory(request.Head.Platform).MsgSecCheck(request.Data.Content);
            if (!msgSec)
            {
                return new Response(ErrCodeEnum.MessageCheckError);
            }
            var response = new Response();
            var moment = new MomentEntity()
            {
                MomentId = Guid.NewGuid(),
                UId = request.Head.UId,
                IsDelete = false,
                IsHide = request.Data.IsHide,
                IsOffLine = request.Data.IsOffLine,
                HidingNickName = request.Data.HidingNickName,
                State = MomentStateEnum.正常发布中,
                NeedCount = request.Data.NeedCount,
                Place = request.Data.Place,
                ExpectGender = request.Data.ExpectGender,
                ShareType = request.Data.ShareType,
                Title = request.Data.Title,
                Content = request.Data.Content,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now
            };
            if (!string.IsNullOrEmpty(request.Data.StopTime))
            {
                moment.StopTime = DateTime.Parse(request.Data.StopTime);
            }
            momentDao.Insert(moment);
            return response;
        }
    }
}
