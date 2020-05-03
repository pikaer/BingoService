using System;

namespace Bingo.Model.Contract
{
    public class MomentActionRequest
    {
        /// <summary>
        /// 动态Id
        /// </summary>
        public Guid MomentId { get; set; }

        /// <summary>
        /// stop=停止,delete=删除
        /// </summary>
        public string Action { get; set; }
    }
}
