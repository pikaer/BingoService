using System;
using System.Collections.Generic;
using System.Text;

namespace Bingo.Model.Contract
{
    public class CancelAskRequest
    {
        /// <summary>
        /// 动态Id
        /// </summary>
        public Guid MomentId { get; set; }
    }
}
