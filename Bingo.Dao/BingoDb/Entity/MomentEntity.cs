using System;

namespace Bingo.Dao.BingoDb.Entity
{
    /// <summary>
    /// 用户发布的动态库
    /// </summary>
    public class MomentEntity : EntityBase
    {
        /// <summary>
        /// 动态Id
        /// </summary>
        public Guid MomentId { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public long UId { get; set; }

        /// <summary>
        /// 主人是否已删除
        /// </summary>
        public bool IsDelete { get; set; }

        /// <summary>
        /// 是否线下活动，默认false
        /// </summary>
        public bool IsOffLine { get; set; }

        /// <summary>
        /// 是否隐身
        /// </summary>
        public bool IsHide { get; set; }

        /// <summary>
        /// 匿名时候对应的昵称
        /// </summary>
        public string HidingNickName { get; set; }

        public MomentStateEnum State{ get; set; }
    }
}
