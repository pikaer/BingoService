using Bingo.Dao.BingoDb.Entity;
using Bingo.Model.Common;
using System.Collections.Generic;

namespace Bingo.Model.Contract
{
    public class MyPublishListRequest
    {
        /// <summary>
        /// 页码（分页传递数据）
        /// </summary>
        public int PageIndex { get; set; }
    }

    public class MyPublishListResponse
    {
        /// <summary>
        /// 是否最后一页
        /// </summary>
        public bool IsLastPage { get; set; }

        public List<MyPublishMomentDetailType> MomentList { get; set; }
    }

}
