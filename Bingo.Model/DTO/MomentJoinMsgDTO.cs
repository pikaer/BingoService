using Bingo.Model.Base;

namespace Bingo.Model.DTO
{
    /// <summary>
    /// 报名审核通知
    /// </summary>
    public class MomentJoinMsgDTO
    {
        /// <summary>
        /// 活动名称
        /// </summary>
        public Value thing1 { get; set; }

        /// <summary>
        /// 申请人
        /// </summary>
        public Value thing2 { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public Value thing3 { get; set; }
    }
}
