using Bingo.Biz.Impl.Builder;
using Bingo.Biz.Interface;
using Bingo.Dao.BingoDb.Dao;
using Bingo.Dao.BingoDb.Dao.Impl;
using Bingo.Dao.BingoDb.Entity;
using Bingo.Model.Base;
using Bingo.Model.Contract;
using Infrastructure;
using System;
using System.Linq;

namespace Bingo.Biz.Impl
{
    public class AskBiz : IAskBiz
    {
        private readonly IMomentDao momentDao = SingletonProvider<MomentDao>.Instance;
        private readonly IApplyInfoDao applyInfoDao = SingletonProvider<ApplyInfoDao>.Instance;
        private readonly IApplyDetailDao applyDetailDao = SingletonProvider<ApplyDetailDao>.Instance;
        private readonly IUserInfoBiz uerInfoBiz = SingletonProvider<UserInfoBiz>.Instance;

        public Response Ask(RequestContext<AskActivityRequest> request)
        {
            MomentEntity moment=momentDao.GetMomentByMomentId(request.Data.MomentId);
            if (moment == null)
            {
                return new Response(ErrCodeEnum.DataIsnotExist,"活动不存在");
            }

            var appLyList = applyInfoDao.GetListByMomentId(request.Data.MomentId);
            if (appLyList.NotEmpty())
            {
                var myApply = appLyList.FirstOrDefault(a => a.UId == request.Head.UId);
                if (myApply != null)
                {
                    return new Response(ErrCodeEnum.Failure, "你已申请过，不能重复申请");
                }
                var usableList = appLyList.Where(a => a.ApplyState == ApplyStateEnum.申请通过).ToList();
                if (usableList.NotEmpty() && usableList.Count >= moment.NeedCount)
                {
                    return new Response(ErrCodeEnum.Failure, "人数已满，无法申请");
                }
            }

            if (moment.IsDelete || moment.State!= MomentStateEnum.正常发布中|| MomentContentBuilder.IsOverTime(moment.StopTime))
            {
                return new Response(ErrCodeEnum.IsOverTime, "活动已失效");
            }
            
            var dto = new ApplyInfoEntity()
            {
                ApplyId = Guid.NewGuid(),
                MomentId = moment.MomentId,
                MomentUId = moment.UId,
                ApplyState = ApplyStateEnum.申请中,
                Source = request.Data.Source,
                UId=request.Head.UId,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now
            };
            applyInfoDao.Insert(dto);

            string remark = "申请加入该活动";
            if (!string.IsNullOrEmpty(request.Data.Remark))
            {
                remark = request.Data.Remark;
            }

            InsertDetail(moment.MomentId,dto.ApplyId, request.Head.UId, remark);

            var momentUser = uerInfoBiz.GetUserInfoByUid(moment.UId);
            if (momentUser != null)
            {
                //发送通知
                AppFactory.Factory(momentUser.Platform).Send_Moment_Join_MsgAsync(moment, request.Head.UId, momentUser.OpenId);
            }
            return new Response(ErrCodeEnum.Success, "申请提交成功");
        }

        public Response AskAction(RequestContext<AskActionRequest> request)
        {
            var applyInfo = applyInfoDao.GetByApplyId(request.Data.ApplyId);
            if (applyInfo == null)
            {
                return new Response(ErrCodeEnum.DataIsnotExist, "申请不存在");
            }
            string remark =string.Empty;
            bool sendMsg = false;
            bool joinSuccess = true;
            if (string.Equals(request.Data.Action, "cancel"))
            {
                applyInfoDao.UpdateState(ApplyStateEnum.申请已撤销, applyInfo.ApplyId);
                remark="撤销活动申请";
            }
            if (string.Equals(request.Data.Action, "reask"))
            {
                applyInfoDao.UpdateState(ApplyStateEnum.申请中, applyInfo.ApplyId);
                remark = "重新申请加入活动";
            }
            if (string.Equals(request.Data.Action, "pass"))
            {
                applyInfoDao.UpdateState(ApplyStateEnum.申请通过, applyInfo.ApplyId);
                momentDao.UpdateApplyCount(applyInfo.MomentId);
                remark = "通过了活动申请";
                sendMsg = true;
            }
            if (string.Equals(request.Data.Action, "black"))
            {
                applyInfoDao.UpdateState(ApplyStateEnum.永久拉黑, applyInfo.ApplyId);
                remark = "拉黑了活动申请";
                sendMsg = true;
                joinSuccess = false;
            }
            if (string.Equals(request.Data.Action, "refuse"))
            {
                applyInfoDao.UpdateState(ApplyStateEnum.被拒绝, applyInfo.ApplyId);
                if (!string.IsNullOrEmpty(request.Data.Remark))
                {
                    remark = request.Data.Remark;
                }
                sendMsg = true;
                joinSuccess = false;
            }
            InsertDetail(applyInfo.MomentId,request.Data.ApplyId, request.Head.UId, remark);

            UserInfoEntity userInfo = uerInfoBiz.GetUserInfoByUid(applyInfo.UId);
            MomentEntity momentInfo = momentDao.GetMomentByMomentId(applyInfo.MomentId);
            if(sendMsg&& userInfo!=null&& momentInfo != null)
            {
                //发送通知
                AppFactory.Factory(userInfo.Platform).Send_Activity_Join_MsgAsync(momentInfo, userInfo.UId, joinSuccess, remark);
            }
            return new Response(ErrCodeEnum.Success, "提交成功");
        }

        private void InsertDetail(Guid momentId, Guid applyId, long uId,string remark)
        {
            var detail = new ApplyDetailEntity()
            {
                ApplyDetailId = Guid.NewGuid(),
                ApplyId = applyId,
                UId = uId,
                MomentId=momentId,
                Type = ApplyDetailTypeEnum.活动申请详情,
                Content = remark,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now
            };
            applyDetailDao.Insert(detail);
        }

        public ResponseContext<AskMomentDetailResponse> AskMomentDetail(Guid applyId, RequestHead head)
        {
            var response = new ResponseContext<AskMomentDetailResponse>();
            var applyInfo = applyInfoDao.GetByApplyId(applyId);
            if (applyInfo == null)
            {
                return response;
            }
            var myUserInfo = uerInfoBiz.GetUserInfoByUid(applyInfo.MomentUId);
            var userInfo = uerInfoBiz.GetUserInfoByUid(applyInfo.UId);
            var moment = MomentBuilder.GetMoment(applyInfo.MomentId);
            if (myUserInfo==null|| userInfo == null|| moment==null)
            {
                return response;
            }
            response.Data = new AskMomentDetailResponse()
            {
                ApplyState = applyInfo.ApplyState,
                ApplyStateDesc = ApplyStateMap(applyInfo.ApplyState),
                IsOverTime = MomentContentBuilder.IsOverTime(moment.StopTime),
                MomentId = moment.MomentId,
                ShareTitle = MomentContentBuilder.GetShareTitle(moment),
                Address = moment.Address,
                Latitude = moment.Latitude,
                Longitude = moment.Longitude,
                IsOffLine = moment.IsOffLine,
                TextColor = ApplyBuilder.TextColorMap(applyInfo.ApplyState),
                UserInfo = UserInfoBuilder.BuildUserInfo(userInfo, head),
                BtnVisable= applyInfo.ApplyState== ApplyStateEnum.申请中,
                ApplyList = ApplyBuilder.GetApplyDetails(applyInfo.ApplyId, head,true)
            };
            return response;
        }



        private string ApplyStateMap(ApplyStateEnum applyState)
        {
            switch (applyState)
            {
                case ApplyStateEnum.申请中:
                    return "申请参与";
                case ApplyStateEnum.被拒绝:
                    return "已拒绝";
                case ApplyStateEnum.申请通过:
                    return "已通过";
                case ApplyStateEnum.申请已撤销:
                    return "对方撤销申请";
                case ApplyStateEnum.永久拉黑:
                    return "已拉黑对方";
                default:
                    return "";
            }
        }

    }
}
