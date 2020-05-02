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
    public class AskActivityBiz : IAskActivityBiz
    {
        private readonly IMomentDao momentDao = SingletonProvider<MomentDao>.Instance;
        private readonly IApplyInfoDao applyInfoDao = SingletonProvider<ApplyInfoDao>.Instance;
        private readonly IApplyDetailDao applyDetailDao = SingletonProvider<ApplyDetailDao>.Instance;

        public Response Ask(RequestContext<AskActivityRequest> request)
        {
            MomentEntity moment=momentDao.GetMomentByMomentId(request.Data.MomentId);
            if (moment == null)
            {
                return new Response(ErrCodeEnum.DataIsnotExist,"活动不存在");
            }
            if(moment.IsDelete || moment.State!= MomentStateEnum.正常发布中|| MomentContentBuilder.IsOverTime(moment.StopTime))
            {
                return new Response(ErrCodeEnum.IsOverTime, "活动已失效");
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
                if(usableList.NotEmpty()&& usableList.Count>= moment.NeedCount)
                {
                    return new Response(ErrCodeEnum.Failure, "人数已满，无法申请");
                }
            }

            var dto = new ApplyInfoEntity()
            {
                ApplyId = Guid.NewGuid(),
                MomentId = moment.MomentId,
                MomentUId = moment.UId,
                ApplyState = ApplyStateEnum.申请中,
                Source = request.Data.Source
            };
            applyInfoDao.Insert(dto);
            return new Response(ErrCodeEnum.Success, "申请提交成功");
        }

        public Response AskAction(RequestContext<AskActionRequest> request)
        {
            var applyInfo = applyInfoDao.GetByApplyId(request.Data.ApplyId);
            if (applyInfo == null)
            {
                return new Response(ErrCodeEnum.DataIsnotExist, "申请不存在");
            }
            if (string.Equals(request.Data.Action, "reask"))
            {
                applyInfoDao.UpdateState(ApplyStateEnum.申请中, applyInfo.ApplyId);
            }
            if (string.Equals(request.Data.Action, "pass"))
            {
                applyInfoDao.UpdateState(ApplyStateEnum.申请通过, applyInfo.ApplyId);
            }
            if (string.Equals(request.Data.Action, "black"))
            {
                applyInfoDao.UpdateState(ApplyStateEnum.永久拉黑, applyInfo.ApplyId);
            }
            if (string.Equals(request.Data.Action, "refuse"))
            {
                applyInfoDao.UpdateState(ApplyStateEnum.被拒绝, applyInfo.ApplyId);
            }
            if (!string.IsNullOrEmpty(request.Data.Remark))
            {
                var detail = new ApplyDetailEntity()
                {
                    ApplyDetailId = Guid.NewGuid(),
                    ApplyId = applyInfo.ApplyId,
                    UId = request.Head.UId,
                    Content = request.Data.Remark
                };
                applyDetailDao.Insert(detail);
            }
            return new Response(ErrCodeEnum.Success, "提交成功");
        }

        public Response CancelAsk(RequestContext<CancelAskRequest> request)
        {
            var applyInfo=applyInfoDao.GetByMomentIdAndUId(request.Data.MomentId, request.Head.UId);
            if (applyInfo == null)
            {
                return new Response(ErrCodeEnum.DataIsnotExist, "申请不存在");
            }
            applyInfoDao.UpdateState(ApplyStateEnum.申请已撤销, applyInfo.ApplyId);
            return new Response(ErrCodeEnum.Success, "取消申请成功");
        }
    }
}
