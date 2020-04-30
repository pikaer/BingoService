using Bingo.Model.Common;
using System.Collections.Generic;

namespace Bingo.Model.Contract
{
    public class MomentListRequest
    {
        /// <summary>
        /// 类型, 0:线上，1:线下
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 页码（分页传递数据）
        /// </summary>
        public int PageIndex { get; set; }
    }

    public class MomentListResponse
    {
        /// <summary>
        /// 是否最后一页
        /// </summary>
        public bool IsLastPage { get; set; }

        public List<MomentDetailType> MomentList { get; set; }
    }
}
