using Bingo.Dao.BingoDb.Entity;
using System;

namespace Bingo.Model.Contract
{
    public class UpdateMomentType
    {
        /// <summary>
        /// 动态Id
        /// </summary>
        public Guid MomentId { get; set; }

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
