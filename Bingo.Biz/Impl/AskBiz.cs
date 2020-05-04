﻿using Bingo.Biz.Impl.Builder;
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
            InsertDetail(dto.ApplyId, request.Head.UId, remark);

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
            }
            if (string.Equals(request.Data.Action, "black"))
            {
                applyInfoDao.UpdateState(ApplyStateEnum.永久拉黑, applyInfo.ApplyId);
                remark = "拉黑了活动申请";
            }
            if (string.Equals(request.Data.Action, "refuse"))
            {
                applyInfoDao.UpdateState(ApplyStateEnum.被拒绝, applyInfo.ApplyId);
                if (!string.IsNullOrEmpty(request.Data.Remark))
                {
                    remark = request.Data.Remark;
                }
                
            }
            InsertDetail(request.Data.ApplyId, request.Head.UId, remark);
            return new Response(ErrCodeEnum.Success, "提交成功");
        }

        private void InsertDetail(Guid applyId,long uId,string remark)
        {
            var detail = new ApplyDetailEntity()
            {
                ApplyDetailId = Guid.NewGuid(),
                ApplyId = applyId,
                UId = uId,
                Content = remark,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now
            };
            applyDetailDao.Insert(detail);
        }

        public ResponseContext<AskMomentListResponse> AskMomentList(long uId)
        {
            var response = new ResponseContext<AskMomentListResponse>
            {
                Data = new AskMomentListResponse()
                {
                    MomentList = new List<AskMomentDetailType>()
                }
            };
            var applyList=applyInfoDao.GetListByMomentUId(uId);
            var myUserInfo = uerInfoBiz.GetUserInfoByUid(uId);
            if (applyList.IsNullOrEmpty()|| myUserInfo==null)
            {
                return response;
            }
            
            foreach (var apply in applyList)
            {
                var userInfo = uerInfoBiz.GetUserInfoByUid(apply.UId);
                var moment = MomentBuilder.GetMoment(apply.MomentId);
                if (moment == null || userInfo == null)
                {
                    continue;
                }
                var result = new AskMomentDetailType()
                {
                    ApplyId = apply.ApplyId,
                    ApplyStateDesc = ApplyStateMap(apply.ApplyState),
                    TextColor = ApplyBuilder.TextColorMap(apply.ApplyState),
                    MomentId = moment.MomentId,
                    UserInfo = UserInfoBuilder.BuildUserInfo(myUserInfo),
                    ContentList = MomentContentBuilder.BuilderContent(moment, apply.ApplyState!=ApplyStateEnum.申请通过)
                };
                var detailList = applyDetailDao.GetListByApplyId(apply.ApplyId);
                if (detailList.NotEmpty())
                {
                    detailList = detailList.OrderByDescending(a => a.CreateTime).ToList();
                    var itemUser = uerInfoBiz.GetUserInfoByUid(detailList[0].UId);
                    if (itemUser != null)
                    {
                        string nickName = itemUser.UId == uId ? "我" : itemUser.NickName;
                        if (nickName.Length > 7)
                        {
                            nickName = nickName.Substring(0, 6) + "...";
                        }
                        result.CreateTimeDesc = DateTimeHelper.GetDateDesc(detailList[0].CreateTime, true);
                        result.Remark = string.Format("{0}：{1}", nickName, detailList[0].Content);
                    }
                }
                response.Data.MomentList.Add(result);
            }
            return response;
        }

        public ResponseContext<AskMomentDetailResponse> AskMomentDetail(Guid applyId,long uId)
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
                TextColor = ApplyBuilder.TextColorMap(applyInfo.ApplyState),
                UserInfo = UserInfoBuilder.BuildUserInfo(myUserInfo, moment,true),
                BtnVisable= applyInfo.ApplyState== ApplyStateEnum.申请中,
                ContentList = MomentContentBuilder.BuilderContent2Contact(moment, myUserInfo, applyInfo.ApplyState == ApplyStateEnum.申请通过),
                ApplyList = ApplyBuilder.GetApplyDetails(applyInfo.ApplyId,uId)
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
