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
        /// 已经申请人数
        /// </summary>
        public int ApplyCount { get; set; }

        /// <summary>
        /// 活动截止时间
        /// </summary>
        public DateTime? StopTime { get; set; }

        /// <summary>
        /// 期待性别
        /// </summary>
        public GenderEnum ExpectGender { get; set; }

        public ShareTypeEnum ShareType { get; set; }

        /// <summary>
        /// 活动位置名（针对线下活动）
        /// </summary>
        public string Place { get; set; }

        /// <summary>
        /// 详细地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 纬度，范围为 -90~90，负数表示南纬
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// 经度，范围为 -180~180，负数表示西经
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// 活动主题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 活动说明
        /// </summary>
        public string Content { get; set; }
    }
}
