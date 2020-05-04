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
        /// 用户信息
        /// </summary>
        public UserInfoType UserInfo { get; set; }
        
        /// <summary>
        /// 内容列表
        /// </summary>
        public List<ContentItem> ContentList { get; set; }
    }
}
