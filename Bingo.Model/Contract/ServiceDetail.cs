namespace Bingo.Model.Contract
{
    public class ServiceDetailResponse
    {
        /// <summary>
        /// 待处理动态总数量
        /// </summary>
        public int PendingCount { get; set; }

        /// <summary>
        /// 我审核通过的动态总数
        /// </summary>
        public int TotalDisposeCount { get; set; }

        /// <summary>
        /// 我今天审核通过的总数
        /// </summary>
        public int TodayDisposeCount { get; set; }
    }
}
