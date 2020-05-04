using Bingo.Model.Base;
using Bingo.Model.Contract;
using System;

namespace Bingo.Biz.Interface
{
    public interface IApplyBiz
    {
        ResponseContext<ApplyMomentListResponse> ApplyMomentList(RequestHead head);

        ResponseContext<ApplyMomentDetailResponse> ApplyMomentDetail(Guid applyId, RequestHead head);
    }
}
