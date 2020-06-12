using Bingo.Model.Base;
using Bingo.Model.Contract;

namespace Bingo.Biz.Interface
{
    public interface IUserSpaceBiz
    {
        ResponseContext<SpaceMomentListResponse> SpaceMomentList(RequestContext<SpaceMomentListRequest> request);
    }
}
