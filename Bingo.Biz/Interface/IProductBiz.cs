using Bingo.Model.Base;
using Bingo.Model.Contract;
using System;

namespace Bingo.Biz.Interface
{
    public interface IProductBiz
    {
        ResponseContext<MomentListResponse> MomentList(RequestContext<MomentListRequest> request);

        ResponseContext<MomentDetailResponse> MomentDetail(Guid momentId, RequestHead head);
        
        ResponseContext<ShareDetailResponse> ShareDetail(Guid momentId, RequestHead head);
    }
}
