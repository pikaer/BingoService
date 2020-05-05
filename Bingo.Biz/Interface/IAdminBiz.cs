using Bingo.Model.Base;
using Bingo.Model.Contract;

namespace Bingo.Biz.Interface
{
    public interface IAdminBiz
    {
        ResponseContext<MomentCheckDetailResponse> MomentCheckList(RequestContext<MomentCheckList> request);

        ResponseContext<MomentCheckDetailType> MomentCheckDetail(RequestContext<MomentCheckDetailRequest> request);

        ResponseContext<ServiceDetailResponse> ServiceDetail(RequestHead head);
    }
}
