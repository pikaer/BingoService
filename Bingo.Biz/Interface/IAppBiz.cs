namespace Bingo.Biz.Interface
{
    public interface IAppBiz
    {
        string GetOpenId(string loginCode);

        bool MsgSecCheck(string message);
    }
}
