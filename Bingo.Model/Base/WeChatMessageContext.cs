using System;
using System.Collections.Generic;
using System.Text;

namespace Bingo.Model.Base
{
    public class WeChatMessageContext<T>
    {
        /// <summary>
        /// 接收者（用户）的 openid
        /// </summary>
        public string touser { get; set; }

        /// <summary>
        /// 所需下发的模板消息的id
        /// </summary>
        public string template_id { get; set; }

        public string access_token { get; set; }

        /// <summary>
        /// 点击模板卡片后的跳转页面，仅限本小程序内的页面。支持带参数,（示例index?foo=bar）。该字段不填则模板无跳转。
        /// </summary>
        public string page { get; set; }

        public T data { get; set; }
    }

    public class Value
    {
        public Value(string text)
        {
            value = text;
        }
        public string value { get; set; }
    }
}
