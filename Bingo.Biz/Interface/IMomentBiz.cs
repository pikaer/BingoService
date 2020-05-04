using Bingo.Model.Base;
using Bingo.Model.Contract;
using System;

namespace Bingo.Biz.Interface
{
    public interface IMomentBiz
    {
        Response Publish(RequestContext<PublishMomentRequest> request);

        ResponseContext<MyPublishListResponse> MyPublishList(RequestContext<MyPublishListRequest> request);
        
        ResponseContext<MyPublishMomentDetailType> MyPublishMomentDetail(Guid momentId, RequestHead head);

        Response MomentAction(Guid momentId, string action);
    }
}
