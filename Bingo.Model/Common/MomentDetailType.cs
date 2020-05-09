using System;
using System.Collections.Generic;

namespace Bingo.Model.Common
{
    public class MomentDetailType
    {
        /// <summary>
        /// 动态Id
        /// </summary>
        public Guid MomentId { get; set; }

        /// <summary>
        /// 分享标题
        /// </summary>
        public string ShareTitle { get; set; }

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
        /// 是否线下活动，默认false
        /// </summary>
        public bool IsOffLine { get; set; }

        /// <summary>
        /// 用户信息
        /// </summary>
        public UserInfoType UserInfo { get; set; }
        
        /// <summary>
        /// 内容列表
        /// </summary>
        public List<ContentItem> ContentList { get; set; }
    }
}
