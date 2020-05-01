using System;
using System.Collections.Generic;
using System.Text;

namespace Bingo.Model.DTO
{
    public class MsgSecCheckRequestDTO
    {
        public string access_token { get; set; }

        public string appid { get; set; }

        /// <summary>
        /// 要检测的文本内容，长度不超过 500KB
        /// </summary>
        public string content { get; set; }
    }
}
