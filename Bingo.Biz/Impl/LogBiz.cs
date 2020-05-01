using System;
using Bingo.Biz.Interface;

namespace Bingo.Biz.Impl
{
    public class LogBiz : ILogBiz
    {
        public void Error(string title, string content, Exception ex = null)
        {
            throw new NotImplementedException();
        }

        public void Error(string title, Exception ex)
        {
            throw new NotImplementedException();
        }

        public void ErrorAsync(string title, string content, Exception ex = null)
        {
            throw new NotImplementedException();
        }

        public void ErrorAsync(string title, Exception ex)
        {
            throw new NotImplementedException();
        }

        public void Info(string title, string content)
        {
            throw new NotImplementedException();
        }

        public void InfoAsync(string title, string content)
        {
            throw new NotImplementedException();
        }

        public void Warn(string title, string content, Exception ex = null)
        {
            throw new NotImplementedException();
        }

        public void WarnAsync(string title, string content, Exception ex = null)
        {
            throw new NotImplementedException();
        }
    }
}
