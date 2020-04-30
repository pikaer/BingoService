using System.Collections.Generic;

namespace Bingo.Model.Common
{
    public class MomentDetailType
    {
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
