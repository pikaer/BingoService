using Bingo.Biz.Interface;
using Bingo.Dao.BingoDb.Dao;
using Bingo.Dao.BingoDb.Dao.Impl;
using Bingo.Dao.BingoDb.Entity;
using Bingo.Model.Base;
using Bingo.Model.Contract;
using Infrastructure;
using System;

namespace Bingo.Biz.Impl
{
    public class MomentActionBiz : IMomentActionBiz
    {
        private readonly IMomentDao momentDao = SingletonProvider<MomentDao>.Instance;

        public Response Publish(RequestContext<PublishMomentRequest> request)
        {
            bool msgSec=AppFactory.Factory(request.Head.Platform).MsgSecCheck(request.Data.Content);
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
                NeedCount=request.Data.NeedCount,
                Place=request.Data.Place,
                ExpectGender = request.Data.ExpectGender,
                ShareType = request.Data.ShareType,
                Content = request.Data.Content,
                CreateTime=DateTime.Now,
                UpdateTime=DateTime.Now
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
