using Bingo.Model.Base;
using Bingo.Model.Contract;
using System;

namespace Bingo.Biz.Interface
{
    public interface IAskBiz
    {
        Response Ask(RequestContext<AskActivityRequest> request);

        Response AskAction(RequestContext<AskActionRequest> request);

        ResponseContext<AskMomentListResponse> AskMomentList(long uId);

        ResponseContext<AskMomentDetailResponse> AskMomentDetail(Guid applyId, long uId);
    }
}
