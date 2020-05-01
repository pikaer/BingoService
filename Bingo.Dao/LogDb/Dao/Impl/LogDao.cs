using Bingo.Dao.LogDb.Entity;
using System;
using Dapper;
using System.Collections.Generic;

namespace Bingo.Dao.LogDb.Dao.Impl
{
    public class LogDao : DbBase,ILogDao
    {
        protected override DbEnum GetDbEnum()
        {
            return DbEnum.LogDb;
        }

        public List<LogEntity> GetLogInfoList()
        {
            throw new NotImplementedException();
        }

        public int InsertLog(LogEntity logEntity)
        {
            var sql = @"INSERT INTO dbo.Log
                                  (LogId
                                  ,LogLevel
                                  ,TransactionID
                                  ,UId
                                  ,Platform
                                  ,Title
                                  ,Content
                                  ,ServiceName
                                  ,CreateTime)
                            VALUES
                                  (@LogId
                                  ,@LogLevel
                                  ,@TransactionID
                                  ,@UId
                                  ,@Platform
                                  ,@Title
                                  ,@Content
                                  ,@ServiceName
                                  ,@CreateTime)";
            using var Db = GetDbConnection();
            return Db.Execute(sql, logEntity);
        }

        
    }
}
