using Bingo.Model.Base;
using Bingo.Model.Contract;

namespace Bingo.Biz.Interface
{
    public interface IMomentListBiz
    {
        ResponseContext<MomentListResponse> MomentList(RequestContext<MomentListRequest> request);
    }
}
