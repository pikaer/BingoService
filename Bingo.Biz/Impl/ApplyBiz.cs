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
    public class ApplyBiz : IApplyBiz
    {
        private readonly IApplyInfoDao applyInfoDao = SingletonProvider<ApplyInfoDao>.Instance;
        private readonly IUserInfoBiz uerInfoBiz = SingletonProvider<UserInfoBiz>.Instance;

        public ResponseContext<ApplyMomentDetailResponse> ApplyMomentDetail(Guid applyId)
        {
            var response = new ResponseContext<ApplyMomentDetailResponse>();
            var applyInfo = applyInfoDao.GetByApplyId(applyId);
            if (applyInfo == null)
            {
                return response;
            }
            var myUserInfo = uerInfoBiz.GetUserInfoByUid(applyInfo.UId);
            var userInfo = uerInfoBiz.GetUserInfoByUid(applyInfo.MomentUId);
            var moment = MomentBuilder.GetMoment(applyInfo.MomentId);
            if (myUserInfo == null || userInfo == null || moment == null)
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
                UserInfo = UserInfoBuilder.BuildUserInfo(myUserInfo),
                ContentList = MomentContentBuilder.BuilderContent(moment),
                ApplyList = ApplyBuilder.GetApplyDetails(applyInfo.ApplyId)
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
                var userInfo = uerInfoBiz.GetUserInfoByUid(apply.UId);
                var moment = MomentBuilder.GetMoment(apply.MomentId);
                if (moment == null || userInfo == null)
                {
                    continue;
                }
                var result = new ApplyMomentDetailType()
                {
                    ApplyId = apply.ApplyId,
                    ApplyStateDesc = ApplyStateMap(apply.ApplyState),
                    TextColor = ApplyBuilder.TextColorMap(apply.ApplyState),
                    MomentId = moment.MomentId,
                    UserInfo = UserInfoBuilder.BuildUserInfo(myUserInfo),
                    ContentList = MomentContentBuilder.BuilderContent(moment)
                };
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
