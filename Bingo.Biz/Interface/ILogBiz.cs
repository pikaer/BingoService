using Bingo.Model.Base;
using System;

namespace Bingo.Biz.Interface
{
    public interface ILogBiz
    {
        void Info(string title, string content, RequestHead head=null);

        void InfoAsync(string title, string content, RequestHead head = null);

        void Warn(string title, string content = null, Exception ex=null, RequestHead head = null);

        void WarnAsync(string title, string content=null, Exception ex = null, RequestHead head = null);

        void Error(string title, string content, Exception ex = null, RequestHead head = null);

        void ErrorAsync(string title, string content, Exception ex = null, RequestHead head = null);

        void Error(string title, Exception ex, RequestHead head = null);

        void ErrorAsync(string title, Exception ex, RequestHead head = null);
    }
}
