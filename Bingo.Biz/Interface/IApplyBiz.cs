using Bingo.Model.Base;
using Bingo.Model.Contract;
using System;

namespace Bingo.Biz.Interface
{
    public interface IApplyBiz
    {
        ResponseContext<ApplyMomentListResponse> ApplyMomentList(long uId);
        ResponseContext<ApplyMomentDetailResponse> ApplyMomentDetail(Guid applyId,long uId);
    }
}
