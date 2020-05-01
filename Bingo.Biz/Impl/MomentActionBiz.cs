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
        private readonly IMomentContentDao momentContentDao = SingletonProvider<MomentContentDao>.Instance;

        public bool Publish(RequestContext<PublishMomentRequest> request)
        {
            var moment = new MomentEntity()
            {
                MomentId = Guid.NewGuid(),
                UId = request.Head.UId,
                IsDelete = false,
                IsHide = request.Data.IsHide,
                IsOffLine = request.Data.IsOffLine,
                HidingNickName = request.Data.HidingNickName,
                State = MomentStateEnum.审核中
            };
            momentDao.Insert(moment);
            foreach(var item in request.Data.ContentList)
            {
                var momentEntity = new MomentContentEntity()
                {
                    MomentId = moment.MomentId,
                    MomentContentId = Guid.NewGuid(),
                    Title = item.Title,
                    Content = item.Content,
                    TagType = item.TagType,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now
                };
                momentContentDao.Insert(momentEntity);
            }
            return true;
        }
    }
}
