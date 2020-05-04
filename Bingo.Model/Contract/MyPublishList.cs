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

        public List<MyPublishMomentDetailType> MomentList { get; set; }
    }

}
