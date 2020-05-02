using Bingo.Model.Base;
using Bingo.Model.Contract;

namespace Bingo.Biz.Interface
{
    public interface IMomentActionBiz
    {
        Response Publish(RequestContext<PublishMomentRequest> request);
    }
}
