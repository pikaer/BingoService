using Bingo.Model.Common;
using System;
using System.Collections.Generic;

namespace Bingo.Model.Contract
{
    public class ApplyMomentListRequest
    {
        /// <summary>
        /// 页码（分页传递数据）
        /// </summary>
        public int PageIndex { get; set; }
    }

    public class ApplyMomentListResponse
    {
        /// <summary>
        /// 是否最后一页
        /// </summary>
        public bool IsLastPage { get; set; }

        public List<ApplyMomentDetailType> MomentList { get; set; }
    }

    public class ApplyMomentDetailType : MomentDetailType
    {
        /// <summary>
        /// 申请Id
        /// </summary>
        public Guid ApplyId { get; set; }

        /// <summary>
        /// 状态描述
        /// </summary>
        public string ApplyStateDesc { get; set; }

        /// <summary>
        /// 状态文本采用字体颜色
        /// </summary>
        public string TextColor { get; set; }
    }

}
