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

        public ResponseContext<ApplyMomentDetailResponse> ApplyMomentDetail(Guid applyId, RequestHead head)
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
                ShareTitle = moment.Content,
                BtnText = btnText,
                Address = moment.Address,
                Latitude = moment.Latitude,
                Longitude = moment.Longitude,
                IsOffLine = moment.IsOffLine,
                NextAction = ApplyBuilder.BtnActionMap(applyInfo.ApplyState),
                BtnVisable =!string.IsNullOrEmpty(btnText),
                TextColor = ApplyBuilder.TextColorMap(applyInfo.ApplyState),
                UserInfo = UserInfoBuilder.BuildUserInfo(userInfo, head),
                ContentList = MomentContentBuilder.BuilderContent(moment,applyInfo.ApplyState!=ApplyStateEnum.申请通过),
                ApplyList = ApplyBuilder.GetApplyDetails(applyInfo.ApplyId, head,false)
            };
            return response;
        }

        public ResponseContext<ApplyMomentListResponse> ApplyMomentList(RequestHead head)
        {
            var response = new ResponseContext<ApplyMomentListResponse>
            {
                Data = new ApplyMomentListResponse()
                {
                    MomentList = new List<ApplyMomentDetailType>()
                }
            };
            var applyList = applyInfoDao.GetListByUId(head.UId);
            if (applyList.IsNullOrEmpty())
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
                    ShareTitle = moment.Content,
                    MomentId = moment.MomentId,
                    Address = moment.Address,
                    Latitude = moment.Latitude,
                    Longitude = moment.Longitude,
                    IsOffLine = moment.IsOffLine,
                    UserInfo = UserInfoBuilder.BuildUserInfo(momentUserInfo, head),
                    ContentList = MomentContentBuilder.BuilderContent2Contact(moment, momentUserInfo,apply.ApplyState==ApplyStateEnum.申请通过)
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
