using Bingo.Dao.BingoDb.Entity;
using Dapper;
using System;
using System.Collections.Generic;

namespace Bingo.Dao.BingoDb.Dao.Impl
{
    public class ApplyDetailDao : DbBase, IApplyDetailDao
    {
        private readonly string SELECT_ApplyInfoEntity = "SELECT ApplyDetailId,ApplyId,UId,Content,CreateTime,UpdateTime FROM dbo.ApplyDetail ";

        protected override DbEnum GetDbEnum()
        {
            return DbEnum.BingoDb;
        }

        public List<ApplyDetailEntity> GetListByApplyId(Guid applyId)
        {
            var sql = SELECT_ApplyInfoEntity + @" Where ApplyId=@ApplyId";
            using var Db = GetDbConnection();
            return Db.Query<ApplyDetailEntity>(sql, new { ApplyId = applyId }).AsList();
        }

        public bool Insert(ApplyDetailEntity entity)
        {
            var sql = @"INSERT INTO dbo.Moment
                                  (ApplyDetailId
                                  ,ApplyId
                                  ,UId
                                  ,Content
                                  ,CreateTime
                                  ,UpdateTime)
                            VALUES
                                  (@ApplyDetailId
                                  ,@ApplyId
                                  ,@UId
                                  ,@Content
                                  ,@CreateTime
                                  ,@UpdateTime)";
            using var Db = GetDbConnection();
            return Db.Execute(sql, entity) > 0;
        }
    }
}
