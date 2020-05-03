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
    public class ApplyBiz : IApplyBiz
    {
        private readonly IApplyInfoDao applyInfoDao = SingletonProvider<ApplyInfoDao>.Instance;
        private readonly IApplyDetailDao applyDetailDao = SingletonProvider<ApplyDetailDao>.Instance;
        private readonly IUserInfoBiz uerInfoBiz = SingletonProvider<UserInfoBiz>.Instance;

        public ResponseContext<ApplyMomentDetailResponse> ApplyMomentDetail(Guid applyId,long uId)
        {
            var response = new ResponseContext<ApplyMomentDetailResponse>();
            var applyInfo = applyInfoDao.GetByApplyId(applyId);
            if (applyInfo == null)
            {
                return response;
            }
            var userInfo = uerInfoBiz.GetUserInfoByUid(applyInfo.MomentUId);
            var moment = MomentBuilder.GetMoment(applyInfo.MomentId);
            if (userInfo == null || moment == null)
            {
                return response;
            }
            string btnText = ApplyBuilder.BtnTextMap(applyInfo.ApplyState);
            response.Data = new ApplyMomentDetailResponse()
            {
                ApplyState = applyInfo.ApplyState,
                ApplyStateDesc = ApplyStateMap(applyInfo.ApplyState),
                MomentId = moment.MomentId,
                BtnText= btnText,
                NextAction= ApplyBuilder.BtnActionMap(applyInfo.ApplyState),
                BtnVisable =!string.IsNullOrEmpty(btnText),
                TextColor = ApplyBuilder.TextColorMap(applyInfo.ApplyState),
                UserInfo = UserInfoBuilder.BuildUserInfo(userInfo, null, uId== userInfo.UId),
                ContentList = MomentContentBuilder.BuilderContent(moment,applyInfo.ApplyState!=ApplyStateEnum.申请通过),
                ApplyList = ApplyBuilder.GetApplyDetails(applyInfo.ApplyId, uId)
            };
            return response;
        }

        public ResponseContext<ApplyMomentListResponse> ApplyMomentList(long uId)
        {
            var response = new ResponseContext<ApplyMomentListResponse>
            {
                Data = new ApplyMomentListResponse()
                {
                    MomentList = new List<ApplyMomentDetailType>()
                }
            };
            var applyList = applyInfoDao.GetListByUId(uId);
            var myUserInfo = uerInfoBiz.GetUserInfoByUid(uId);
            if (applyList.IsNullOrEmpty() || myUserInfo == null)
            {
                return response;
            }
            foreach (var apply in applyList)
            {
                var momentUserInfo = uerInfoBiz.GetUserInfoByUid(apply.MomentUId);
                var moment = MomentBuilder.GetMoment(apply.MomentId);
                if (moment == null || momentUserInfo == null)
                {
                    continue;
                }
                
                var result = new ApplyMomentDetailType()
                {
                    ApplyId = apply.ApplyId,
                    ApplyStateDesc = ApplyStateMap(apply.ApplyState),
                    TextColor = ApplyBuilder.TextColorMap(apply.ApplyState),
                    MomentId = moment.MomentId,
                    UserInfo = UserInfoBuilder.BuildUserInfo(momentUserInfo),
                    ContentList = MomentContentBuilder.BuilderContent2Contact(moment, momentUserInfo,apply.ApplyState==ApplyStateEnum.申请通过)
                };
                var detailList = applyDetailDao.GetListByApplyId(apply.ApplyId);
                if (detailList.NotEmpty())
                {
                    detailList = detailList.OrderByDescending(a => a.CreateTime).ToList();
                    var itemUser= uerInfoBiz.GetUserInfoByUid(detailList[0].UId);
                    if (itemUser != null)
                    {
                        string nickName = itemUser.UId == uId ? "我" : itemUser.NickName;
                        if (nickName.Length > 7)
                        {
                            nickName = nickName.Substring(0, 6) + "...";
                        }
                        result.CreateTimeDesc = DateTimeHelper.GetDateDesc(detailList[0].CreateTime, true);
                        result.Remark = string.Format("{0}：{1}",nickName,detailList[0].Content);
                    }
                }
                response.Data.MomentList.Add(result);
            }
            return response;
        }

        private string ApplyStateMap(ApplyStateEnum applyState)
        {
            switch (applyState)
            {
                case ApplyStateEnum.申请中:
                    return "申请中";
                case ApplyStateEnum.被拒绝:
                    return "申请被拒绝";
                case ApplyStateEnum.申请通过:
                    return "已通过";
                case ApplyStateEnum.申请已撤销:
                    return "已撤销申请";
                case ApplyStateEnum.永久拉黑:
                    return "对方已拉黑申请";
                default:
                    return "";
            }
        }
    }
}
