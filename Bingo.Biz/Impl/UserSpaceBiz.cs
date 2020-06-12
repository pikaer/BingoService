using Bingo.Biz.Impl.Builder;
using Bingo.Biz.Interface;
using Bingo.Dao.BingoDb.Dao;
using Bingo.Dao.BingoDb.Dao.Impl;
using Bingo.Dao.BingoDb.Entity;
using Bingo.Model.Base;
using Bingo.Model.Contract;
using Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace Bingo.Biz.Impl
{
    public class UserSpaceBiz : IUserSpaceBiz
    {
        private readonly IMomentDao momentDao = SingletonProvider<MomentDao>.Instance;
        private readonly IUserInfoBiz uerInfoBiz = SingletonProvider<UserInfoBiz>.Instance;
        private readonly static IApplyInfoDao applyInfoDao = SingletonProvider<ApplyInfoDao>.Instance;


        public ResponseContext<SpaceMomentListResponse> SpaceMomentList(RequestContext<SpaceMomentListRequest> request)
        {
            var response = new ResponseContext<SpaceMomentListResponse>()
            {
                Data = new SpaceMomentListResponse()
                {
                    MomentList = new List<SpaceMomentDetailType>()
                }
            };
            var userInfo = uerInfoBiz.GetUserInfoByUid(request.Data.UId);
            var momentList = momentDao.GetMomentListByUid(request.Data.UId);
            if (momentList.IsNullOrEmpty())
            {
                return response;
            }

            foreach (var moment in momentList.Where(a=>a.State== MomentStateEnum.正常发布中))
            {
                bool overCount = ApplyBuilder.IsOverCount(moment);
                var applyList = applyInfoDao.GetListByMomentId(moment.MomentId);
                var dto = new SpaceMomentDetailType()
                {
                    MomentId = moment.MomentId,
                    ShareTitle = MomentContentBuilder.GetShareTitle(moment),
                    Address = moment.Address,
                    Latitude = moment.Latitude,
                    Longitude = moment.Longitude,
                    IsOffLine = moment.IsOffLine,
                    ApplyCountColor = "black",
                    ApplyCountDesc = ApplyBuilder.GetApplyCountDescV1(applyList),
                    StateDesc = MomentContentBuilder.MomentStateMap(moment.State, moment.StopTime, overCount),
                    TextColor = MomentContentBuilder.TextColorMap(moment.State, moment.StopTime, overCount),
                    UserInfo = UserInfoBuilder.BuildUserInfo(userInfo, request.Head),
                    ContentList = MomentContentBuilder.BuilderContent(moment, false)
                };

                response.Data.MomentList.Add(dto);
            }
            
            return response;
        }
    }
}
