using Bingo.Biz.Impl;
using Bingo.Biz.Interface;
using Bingo.Dao.BingoDb.Entity;
using Infrastructure;

namespace Bingo.Biz
{
    public class AppFactory
    {
        public static IAppBiz Factory(PlatformEnum platform)
        {
            switch (platform)
            {
                case PlatformEnum.QQ_MiniApp:
                    return SingletonProvider<App_QQBiz>.Instance;
                case PlatformEnum.WX_MiniApp:
                default:
                    return SingletonProvider<App_WechatBiz>.Instance;
            }
        }
    }
}
