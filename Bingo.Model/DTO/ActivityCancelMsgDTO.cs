using Bingo.Model.Base;

namespace Bingo.Model.DTO
{
    /// <summary>
    /// 活动取消通知
    /// </summary>
    public class ActivityCancelMsgDTO
    {
        /// <summary>
        /// 活动标题
        /// </summary>
        public Value thing1 { get; set; }

        /// <summary>
        /// 活动时间
        /// </summary>
        public Value date2 { get; set; }

        /// <summary>
        /// 发起人
        /// </summary>
        public Value name3 { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public Value thing4 { get; set; }
    }
}
