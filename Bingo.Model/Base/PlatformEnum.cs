using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Bingo.Model.Base
{
    public enum PlatformEnum
    {
        [Description("微信小程序")]
        WX_MiniApp = 0,

        [Description("QQ小程序")]
        QQ_MiniApp = 1,
    }
}
