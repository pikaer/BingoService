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
    public class MomentActionBiz : IMomentActionBiz
    {
        private readonly IMomentDao momentDao = SingletonProvider<MomentDao>.Instance;
        private readonly IApplyInfoDao applyInfoDao = SingletonProvider<ApplyInfoDao>.Instance;
        private readonly IUserInfoBiz uerInfoBiz = SingletonProvider<UserInfoBiz>.Instance;

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
                var dto = new MyPublishMomentDetailType()
                {
                    MomentId = moment.MomentId,
                    State = moment.State,
                    ShareFlag= moment.State== MomentStateEnum.正常发布中,
                    StateDesc = MomentContentBuilder.MomentStateMap(moment.State),
                    TextColor= MomentContentBuilder.TextColorMap(moment.State),
                    UserInfo = UserInfoBuilder.BuildUserInfo(userInfo, moment),
                    ContentList = MomentContentBuilder.BuilderContent(moment)
                };
                response.Data.MomentList.Add(dto);
            }
            return response;
        }

        public ResponseContext<MyPublishMomentDetailType> MyPublishMomentDetail(Guid momentId)
        {
            var moment = momentDao.GetMomentByMomentId(momentId);
            if (moment == null)
            {
                return new ResponseContext<MyPublishMomentDetailType>(ErrCodeEnum.DataIsnotExist);
            }
            return new ResponseContext<MyPublishMomentDetailType>()
            {
                Data = new MyPublishMomentDetailType()
                {
                    MomentId = momentId,
                    State = moment.State,
                    ShareFlag = moment.State == MomentStateEnum.正常发布中,
                    StateDesc = MomentContentBuilder.MomentStateMap(moment.State),
                    TextColor = MomentContentBuilder.TextColorMap(moment.State),
                    UserInfo = UserInfoBuilder.BuildUserInfo(uerInfoBiz.GetUserInfoByUid(moment.UId), moment),
                    ContentList = MomentContentBuilder.BuilderContent(moment),
                    ApplyList= GetApplyList(momentId)
                }
            };
        }

        private List<ApplyDetailItem> GetApplyList(Guid momentId)
        {
            var applyList = applyInfoDao.GetListByMomentId(momentId);
            if (applyList.IsNullOrEmpty())
            {
                return null;
            }
            var resultList = new List<ApplyDetailItem>();
            foreach (var apply in applyList)
            {
                var userInfo = uerInfoBiz.GetUserInfoByUid(apply.UId);
                if (userInfo == null)
                {
                    continue;
                }
                var result = new ApplyDetailItem()
                {
                    UserInfo=UserInfoBuilder.BuildUserInfo(userInfo),
                    CreateTimeDesc = DateTimeHelper.GetDateDesc(userInfo.CreateTime, true),
                };
                resultList.Add(result);
            }
            return resultList;
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
