using System;

namespace Bingo.Biz.Interface
{
    public interface ILogBiz
    {
        void Info(string title, string content);

        void InfoAsync(string title, string content);

        void Warn(string title, string content = null, Exception ex=null);

        void WarnAsync(string title, string content=null, Exception ex = null);

        void Error(string title, string content, Exception ex = null);

        void ErrorAsync(string title, string content, Exception ex = null);

        void Error(string title, Exception ex);

        void ErrorAsync(string title, Exception ex);
    }
}
