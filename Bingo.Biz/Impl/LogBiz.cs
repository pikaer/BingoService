using Bingo.Biz.Interface;
using Bingo.Dao.LogDb.Dao;
using Bingo.Dao.LogDb.Dao.Impl;
using Bingo.Dao.LogDb.Entity;
using Bingo.Model.Base;
using Infrastructure;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Bingo.Biz.Impl
{
    public class LogBiz : ILogBiz
    {
        private readonly ILogDao logDao = SingletonProvider<LogDao>.Instance;

        public void Error(string title, string content, Exception ex = null, RequestHead head = null)
        {
            var entity = GetLogEntity(head);
            entity.LogLevel = LogLevelEnum.Error;
            entity.Title = title;
            var stringBuilder = new StringBuilder();
            if (!string.IsNullOrEmpty(content))
            {
                stringBuilder.Append(content);
            }
            if (ex != null)
            {
                stringBuilder.Append(ex.ToString());
            }
            logDao.InsertLog(entity);
        }

        public void Error(string title, Exception ex, RequestHead head = null)
        {
            var entity = GetLogEntity(head);
            entity.LogLevel = LogLevelEnum.Error;
            entity.Title = title;
            var stringBuilder = new StringBuilder();
            if (ex != null)
            {
                stringBuilder.Append(ex.ToString());
            }
            entity.Content = stringBuilder.ToString();
            logDao.InsertLog(entity);
        }

        public void ErrorAsync(string title, string content, Exception ex = null, RequestHead head = null)
        {
            Task.Factory.StartNew(() =>
            {
                Error(title, content, ex, head);
            });
        }

        public void ErrorAsync(string title, Exception ex, RequestHead head = null)
        {
            Task.Factory.StartNew(() =>
            {
                Error(title, ex, head);
            });
        }

        public void Info(string title, string content, RequestHead head = null)
        {
            var entity = GetLogEntity(head);
            entity.LogLevel = LogLevelEnum.Info;
            entity.Title = title;
            entity.Content = content;
            logDao.InsertLog(entity);
        }

        public void InfoAsync(string title, string content, RequestHead head = null)
        {
            Task.Factory.StartNew(() =>
            {
                Info(title, content, head);
            });
        }

        public void Warn(string title, string content, Exception ex = null, RequestHead head = null)
        {
            var entity = GetLogEntity(head);
            entity.LogLevel = LogLevelEnum.Warn;
            entity.Title = title;
            var stringBuilder = new StringBuilder();
            if (!string.IsNullOrEmpty(content))
            {
                stringBuilder.Append(content);
            }
            if (ex != null)
            {
                stringBuilder.Append(ex.ToString());
            }
            logDao.InsertLog(entity);
        }

        public void WarnAsync(string title, string content, Exception ex = null, RequestHead head = null)
        {
            Task.Factory.StartNew(() =>
            {
                Warn(title, content, ex, head);
            });
        }

        private LogEntity GetLogEntity(RequestHead head)
        {
            var log= new LogEntity()
            {
                LogId=Guid.NewGuid(),
                LogLevel= LogLevelEnum.Info,
                TransactionID=Guid.NewGuid(),
                UId=0,
                Platform="",
                Title="",
                Content="",
                ServiceName= JsonSettingHelper.AppSettings["ServiceName"],
                CreateTime=DateTime.Now
            };
            if (head != null)
            {
                log.UId = head.UId;
                log.TransactionID = head.TransactionId;
                log.Platform = head.Platform.ToString();
            }
            return log;
        }
    }
}
