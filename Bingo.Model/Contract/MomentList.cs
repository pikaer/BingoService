using Bingo.Dao.BingoDb.Entity;
using Bingo.Model.Common;
using System.Collections.Generic;

namespace Bingo.Model.Contract
{
    public class MomentListRequest
    {
        public bool OffLine { get; set; }

        /// <summary>
        /// 页码（分页传递数据）
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 用户的性别，值为1时是男性，值为2时是女性，值为0时是未知,此时一般是用户未授权
        /// </summary>
        public GenderEnum Gender { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public SchoolStateEnum SchoolState { get; set; }

        public AgeFilter Age { get; set; }
    }

    public class MomentListResponse
    {
        /// <summary>
        /// 是否最后一页
        /// </summary>
        public bool IsLastPage { get; set; }

        public List<MomentDetailType> MomentList { get; set; }
    }

    public class AgeFilter
    {
        public bool All { get; set; }

        public bool After05 { get; set; }

        public bool After00 { get; set; }

        public bool After95 { get; set; }

        public bool After90 { get; set; }

        public bool After85 { get; set; }

        public bool After80 { get; set; }

        public bool Before80 { get; set; }
    }
}
