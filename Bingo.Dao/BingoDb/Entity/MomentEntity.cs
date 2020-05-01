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

        /// <summary>
        /// 限定人数
        /// </summary>
        public int NeedCount { get; set; }

        /// <summary>
        /// 活动截止时间
        /// </summary>
        public DateTime? StopTime { get; set; }

        /// <summary>
        /// 活动位置（针对线下活动）
        /// </summary>
        public string Place { get; set; }

        /// <summary>
        /// 期待性别
        /// </summary>
        public GenderEnum ExpectGender { get; set; }

        public ShareTypeEnum ShareType { get; set; }
    }
}
