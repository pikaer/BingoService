﻿using System.ComponentModel;

namespace Bingo.Dao.BingoDb.Entity
{
    public enum PlatformEnum
    {
        [Description("微信小程序")]
        WX_MiniApp = 0,

        [Description("QQ小程序")]
        QQ_MiniApp = 1,
    }
}
