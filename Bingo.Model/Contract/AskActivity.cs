using System;

namespace Bingo.Model.Contract
{
    public class AskActivityRequest
    {
        /// <summary>
        /// 动态Id
        /// </summary>
        public Guid MomentId { get; set; }

        /// <summary>
        /// 请求来源
        /// </summary>
        public string Source { get; set; }
    }
}
