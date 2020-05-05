using System;
using System.Collections.Generic;

namespace Bingo.Model.Common
{
    public class ApplyItem
    {
        /// <summary>
        /// 申请Id
        /// </summary>
        public Guid ApplyId { get; set; }

        /// <summary>
        /// 用户信息
        /// </summary>
        public UserInfoType UserInfo { get; set; }

        /// <summary>
        /// 提交时间
        /// </summary>
        public string CreateTimeDesc { get; set; }

        /// <summary>
        /// 状态文本颜色
        /// </summary>
        public string StateDesc { get; set; }

        /// <summary>
        /// 状态文本采用字体颜色
        /// </summary>
        public string TextColor { get; set; }

        /// <summary>
        /// 申请内容
        /// </summary>
        public string Content { get; set; }

        public bool ShowBorder { get; set; }
    }
}
