using Bingo.Model.Common;
using System.Collections.Generic;

namespace Bingo.Model.Contract
{
    public class SpaceMomentListRequest
    {
        public long UId { get; set; }
    }

    public class SpaceMomentListResponse
    {
        public List<SpaceMomentDetailType> MomentList { get; set; }
    }

    public class SpaceMomentDetailType : MomentDetailType
    {
        /// <summary>
        /// 状态描述
        /// </summary>
        public string StateDesc { get; set; }

        /// <summary>
        /// 状态文本采用字体颜色
        /// </summary>
        public string TextColor { get; set; }

        /// <summary>
        /// 申请人数描述
        /// </summary>
        public string ApplyCountDesc { get; set; }

        public string ApplyCountColor { get; set; }
    }

}
