using Bingo.Model.Base;
using Bingo.Model.Contract;

namespace Bingo.Biz.Interface
{
    public interface IAskActivityBiz
    {
        Response Ask(RequestContext<AskActivityRequest> request);
        Response CancelAsk(RequestContext<CancelAskRequest> request);
        Response AskAction(RequestContext<AskActionRequest> request);
    }
}
