using Bingo.Model.Base;
using Bingo.Model.Contract;

namespace Bingo.Biz.Interface
{
    public interface IMomentListBiz
    {
        /// <summary>
        /// 动态列表
        /// </summary>
        ResponseContext<MomentListResponse> MomentList(RequestContext<MomentListRequest> request);
    }
}
