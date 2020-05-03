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
    public class ProductBiz : IProductBiz
    {
        private readonly ILogBiz log= SingletonProvider<LogBiz>.Instance;
        private readonly IUserInfoBiz uerInfoBiz = SingletonProvider<UserInfoBiz>.Instance;
        private readonly IMomentDao momentDao = SingletonProvider<MomentDao>.Instance;
        private readonly static IApplyInfoDao applyInfoDao = SingletonProvider<ApplyInfoDao>.Instance;

        public ResponseContext<MomentListResponse> MomentList(RequestContext<MomentListRequest> request)
        {
            try
            {
                var response = new ResponseContext<MomentListResponse>
                {
                    Data = new MomentListResponse()
                    {
                        MomentList=new List<MomentDetailType>()
                    }
                };
                List<MomentEntity> moments = momentDao.GetMomentListByParam();
                if (moments.IsNullOrEmpty())
                {
                    return response;
                }
                foreach(var moment in moments)
                {
                    var userInfo = uerInfoBiz.GetUserInfoByUid(moment.UId);
                    if (userInfo == null)
                    {
                        continue;
                    }
                    var dto = new MomentDetailType()
                    {
                        MomentId=moment.MomentId,
                        UserInfo = UserInfoBuilder.BuildUserInfo(userInfo, moment, request.Head.UId==userInfo.UId),
                        ContentList= MomentContentBuilder.BuilderContent(moment)
                    };
                    response.Data.MomentList.Add(dto);
                }
                return response;
            }
            catch(Exception ex)
            {
                log.ErrorAsync("MomentListBiz.MomentList", ex);
                return null;
            }
        }

        public ResponseContext<MomentDetailResponse> MomentDetail(Guid momentId, long uid)
        {
            var moment = momentDao.GetMomentByMomentId(momentId);
            if (moment == null)
            {
                return new ResponseContext<MomentDetailResponse>(ErrCodeEnum.DataIsnotExist);
            }
            var userInfo = uerInfoBiz.GetUserInfoByUid(moment.UId);
            if (userInfo == null)
            {
                return new ResponseContext<MomentDetailResponse>(ErrCodeEnum.DataIsnotExist);
            }
            var applyInfo = applyInfoDao.GetByMomentIdAndUId(momentId, uid);
            bool isApply = applyInfo != null;
            bool selfFlag = moment.UId == uid;
            string btnText = MomentContentBuilder.BtnTextMap(moment.State, moment.StopTime, isApply, selfFlag, ApplyBuilder.IsOverCount(moment));
            string stateDesc = MomentContentBuilder.MomentStateMap(moment.State, moment.StopTime,ApplyBuilder.IsOverCount(moment));
            
            return new ResponseContext<MomentDetailResponse>()
            {
                Data = new MomentDetailResponse()
                {
                    MomentId = momentId,
                    ApplyId= isApply? applyInfo.ApplyId:Guid.Empty,
                    ShareFlag = moment.State == MomentStateEnum.正常发布中,
                    ApplyFlag = isApply,
                    SelfFlag= selfFlag,
                    BtnText = btnText,
                    StateDesc=stateDesc,
                    AskFlag = string.Equals(btnText, "申请参与"),
                    BtnVisable = !string.IsNullOrEmpty(btnText),
                    TextColor = MomentContentBuilder.TextColorMap(moment.State),
                    UserInfo = UserInfoBuilder.BuildUserInfo(userInfo, moment, userInfo.UId==uid),
                    ContentList = MomentContentBuilder.BuilderContent(moment),
                    ApplyList = ApplyBuilder.GetApplyList(momentId)
                }
            };
        }

    }
}
