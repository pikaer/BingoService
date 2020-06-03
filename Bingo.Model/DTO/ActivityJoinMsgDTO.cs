using Bingo.Model.Base;

namespace Bingo.Model.DTO
{
    /// <summary>
    /// 活动报名通知
    /// </summary>
    public class ActivityJoinMsgDTO
    {
        /// <summary>
        /// 发布内容
        /// </summary>
        public Value thing2 { get; set; }

        /// <summary>
        /// 活动地点
        /// </summary>
        public Value thing3 { get; set; }

        /// <summary>
        /// 当前状态
        /// </summary>
        public Value phrase1 { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public Value thing9 { get; set; }
    }
}
