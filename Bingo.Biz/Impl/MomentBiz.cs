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
using System.Linq;

namespace Bingo.Biz.Impl
{
    public class MomentBiz : IMomentBiz
    {
        private readonly IMomentDao momentDao = SingletonProvider<MomentDao>.Instance;
        private readonly IUserInfoBiz uerInfoBiz = SingletonProvider<UserInfoBiz>.Instance;
        private readonly static IApplyInfoDao applyInfoDao = SingletonProvider<ApplyInfoDao>.Instance;
        private readonly static IUserInfoDao userInfoDao = SingletonProvider<UserInfoDao>.Instance;
        private readonly IApplyDetailDao applyDetailDao = SingletonProvider<ApplyDetailDao>.Instance;

        public Response MomentAction(Guid momentId,string action,string remark,long uId)
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
                    InsertApplyDetail(momentId, "停止活动", uId);
                    break;
                case "delete":
                    momentDao.Delete(momentId);
                    message = "删除成功";
                    break;
                case "complain":
                    remark = "申诉理由：" + remark;
                    momentDao.UpdateState(momentId, MomentStateEnum.审核中);
                    InsertApplyDetail(momentId, remark, uId);
                    message = "提交成功";
                    break;
                case "black":
                case "pass":
                case "refuse":
                    if (moment.State != MomentStateEnum.审核中)
                    {
                        return new Response(ErrCodeEnum.Failure, "用户修改了动态，审核失败");
                    }
                    var userInfo = uerInfoBiz.GetUserInfoByUid(uId);
                    if(userInfo==null|| userInfo.UserType == UserTypeEnum.Default || userInfo.UserType== UserTypeEnum.SimulationUser)
                    {
                        return new Response(ErrCodeEnum.Failure, "权限不足，请联系管理员添加权限");
                    }
                    else
                    {
                        var momentState= MomentStateEnum.正常发布中;
                        if (action.Equals("pass"))
                        {
                            remark = "审核通过，发布成功";
                        }
                        if (action.Equals("black"))
                        {
                            remark = "拉黑此活动申请";
                            momentState = MomentStateEnum.被关小黑屋中;
                        }
                        if (action.Equals("refuse"))
                        {
                            remark = "审核不通过：" + remark;
                            momentState = MomentStateEnum.审核被拒绝;
                        }
                        momentDao.UpdateState(momentId, momentState);
                    }
                    InsertApplyDetail(momentId, remark, uId);
                    message = "操作成功";
                    break;
                default:
                     break;
            }
            return new Response(ErrCodeEnum.Success, message);
        }

        private bool InsertApplyDetail(Guid momentId, string remark, long uId)
        {
            var detail = new ApplyDetailEntity()
            {
                ApplyDetailId = Guid.NewGuid(),
                UId = uId,
                MomentId = momentId,
                Type = ApplyDetailTypeEnum.动态审核详情,
                Content = remark,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now
            };
            return applyDetailDao.Insert(detail);
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
            //有用户申请的动态
            var applyMomentList = new List<MyPublishMomentDetailType>();
            //正常动态
            var commomMomentList = new List<MyPublishMomentDetailType>();
            foreach (var moment in momentList)
            {
                bool overCount = ApplyBuilder.IsOverCount(moment);
                var applyList = applyInfoDao.GetListByMomentId(moment.MomentId);
                var dto = new MyPublishMomentDetailType()
                {
                    MomentId = moment.MomentId,
                    State = moment.State,
                    ShareTitle = moment.Content,
                    Address = moment.Address,
                    Latitude = moment.Latitude,
                    Longitude = moment.Longitude,
                    IsOffLine = moment.IsOffLine,
                    ApplyCountDesc =ApplyBuilder.GetApplyCountDesc(applyList),
                    ApplyCountColor= ApplyBuilder.GetApplyCountColor(applyList),
                    IsOverTime = MomentContentBuilder.IsOverTime(moment.StopTime),
                    ShareFlag = moment.State== MomentStateEnum.正常发布中,
                    StateDesc = MomentContentBuilder.MomentStateMap(moment.State, moment.StopTime, overCount),
                    TextColor= MomentContentBuilder.TextColorMap(moment.State, moment.StopTime, overCount),
                    UserInfo = UserInfoBuilder.BuildUserInfo(userInfo, request.Head),
                    ContentList = MomentContentBuilder.BuilderContent(moment,false)
                };
                if(applyList.NotEmpty()&& applyList.Count(a=>a.ApplyState == ApplyStateEnum.申请中) > 0)
                {
                    applyMomentList.Add(dto);
                }
                else
                {
                    commomMomentList.Add(dto);
                }
            }
            //有用户申请的保证置顶
            if (applyMomentList.NotEmpty())
            {
                response.Data.MomentList.AddRange(applyMomentList);
            }
            if (commomMomentList.NotEmpty())
            {
                response.Data.MomentList.AddRange(commomMomentList);
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
                    ShareTitle = moment.Content,
                    Address = moment.Address,
                    Latitude = moment.Latitude,
                    Longitude = moment.Longitude,
                    IsOverTime = MomentContentBuilder.IsOverTime(moment.StopTime),
                    ShareFlag = moment.State == MomentStateEnum.正常发布中,
                    VerifyStateDesc= MomentContentBuilder.VerifyStateMap(moment.State),
                    StateDesc = MomentContentBuilder.MomentStateMap(moment.State,moment.StopTime, overCount),
                    TextColor = MomentContentBuilder.TextColorMap(moment.State, moment.StopTime, overCount),
                    UserInfo = UserInfoBuilder.BuildUserInfo(userInfo, head),
                    ContentList = MomentContentBuilder.BuilderContent(moment,false),
                    ApplyList= ApplyBuilder.GetApplyList(momentId,false, head,moment.UId),
                    CheckList= ApplyBuilder.GetCheckDetails(moment, userInfo, head)
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
                State = MomentStateEnum.审核中,
                NeedCount = request.Data.NeedCount,
                Place = request.Data.Place,
                Address=request.Data.Address,
                Latitude = request.Data.Latitude,
                Longitude = request.Data.Longitude,
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
            if (!string.IsNullOrEmpty(request.Data.Mobile))
            {
                userInfoDao.UpdateMobile(request.Head.UId, request.Data.Mobile);
            }
            if (!string.IsNullOrEmpty(request.Data.WeChatNo))
            {
                userInfoDao.UpdateMobile(request.Head.UId, request.Data.WeChatNo);
            }
            if (!string.IsNullOrEmpty(request.Data.QQNo))
            {
                userInfoDao.UpdateMobile(request.Head.UId, request.Data.QQNo);
            }
            momentDao.Insert(moment);
            return response;
        }

        public ResponseContext<UpdateMomentType> MomentUpdateDetail(Guid momentId)
        {
            var moment = momentDao.GetMomentByMomentId(momentId);
            if (moment == null)
            {
                return new ResponseContext<UpdateMomentType>(ErrCodeEnum.DataIsnotExist);
            }
            var userInfo = uerInfoBiz.GetUserInfoByUid(moment.UId);
            return new ResponseContext<UpdateMomentType>()
            {
                Data = new UpdateMomentType()
                {
                    MomentId = momentId,
                    IsOffLine = moment.IsOffLine,
                    IsHide = moment.IsHide,
                    Mobile = userInfo.Mobile,
                    WeChatNo = userInfo.WeChatNo,
                    QQNo = userInfo.QQNo,
                    HidingNickName = moment.HidingNickName,
                    NeedCount = moment.NeedCount,
                    StopTime = moment.StopTime.Value.ToString(DateTimeHelper.yMdHm),
                    Place = moment.Place,
                    ExpectGender = moment.ExpectGender,
                    ShareType = moment.ShareType,
                    Address=moment.Address,
                    Latitude = moment.Latitude,
                    Longitude = moment.Longitude,
                    Title = moment.Title,
                    Content = moment.Content
                }
            };
        }

        public Response UpdateMoment(UpdateMomentType moment, long uId)
        {
            var momentInfo = momentDao.GetMomentByMomentId(moment.MomentId);
            bool canEdit = momentInfo.State != MomentStateEnum.审核中 || momentInfo.State != MomentStateEnum.审核被拒绝;
            if (momentInfo==null||!canEdit)
            {
                return new Response(ErrCodeEnum.Failure, "你的动态已审核通过，不能修改，可以删除后重发哦~");
            }
            var entity = new MomentEntity()
            {
                MomentId = moment.MomentId,
                IsOffLine = moment.IsOffLine,
                IsHide = moment.IsHide,
                HidingNickName = moment.HidingNickName,
                NeedCount = moment.NeedCount,
                StopTime = DateTime.Parse(moment.StopTime),
                ExpectGender = moment.ExpectGender,
                ShareType = moment.ShareType,
                Place = moment.Place,
                Address = moment.Address,
                Latitude = moment.Latitude,
                Longitude = moment.Longitude,
                Title = moment.Title,
                Content = moment.Content,
                State=MomentStateEnum.审核中,
                UpdateTime =DateTime.Now
            };
            bool sucess=momentDao.UpdateMoment(entity);
            if (sucess)
            {
                InsertApplyDetail(moment.MomentId, "修改活动内容，重新审核中", uId);
                if (!string.IsNullOrEmpty(moment.Mobile))
                {
                    userInfoDao.UpdateMobile(uId, moment.Mobile);
                }
                if (!string.IsNullOrEmpty(moment.WeChatNo))
                {
                    userInfoDao.UpdateMobile(uId, moment.WeChatNo);
                }
                if (!string.IsNullOrEmpty(moment.QQNo))
                {
                    userInfoDao.UpdateMobile(uId, moment.QQNo);
                }
                return new Response();
            }
            else
            {
                return new Response(ErrCodeEnum.Failure,"修改失败");
            }
        }

        public ResponseContext<UnReadCountResponse> GetUnReadCount(RequestHead head)
        {
            int count = applyInfoDao.GetUnReadCount(head.UId);
            string countStr = "";
            if (0 < count&& count < 100)
            {
                countStr = count.ToString();
            }
            if (count >= 100)
            {
                countStr = "99+";
            }
            return new ResponseContext<UnReadCountResponse>()
            {
                Data = new UnReadCountResponse()
                {
                    UnReadCount = countStr
                }
            };
        }
    }
}
