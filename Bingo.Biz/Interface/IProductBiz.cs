using Bingo.Model.Base;
using Bingo.Model.Contract;
using System;

namespace Bingo.Biz.Interface
{
    public interface IProductBiz
    {
        /// <summary>
        /// 动态列表
        /// </summary>
        ResponseContext<MomentListResponse> MomentList(RequestContext<MomentListRequest> request);

        ResponseContext<MomentDetailResponse> MomentDetail(Guid momentId, long uId);
    }
}
