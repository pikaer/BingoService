using System;

namespace Bingo.Model.Contract
{
    public class AskActionRequest
    {
        /// <summary>
        /// 申请Id
        /// </summary>
        public Guid ApplyId { get; set; }

        /// <summary>
        /// pass=通过,refuse=拒绝,black=拉黑,reask=重新申请,cancel=取消申请
        /// </summary>
        public string Action { get; set; }

        public string Remark { get; set; }
    }
}
