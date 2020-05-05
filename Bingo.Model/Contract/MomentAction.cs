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
        /// stop=停止,delete=删除,pass=审核通过,refuse=审核不通过,
        /// </summary>
        public string Action { get; set; }

        public string Remark { get; set; }
    }
}
