using Bingo.Dao.BingoDb.Entity;
using Dapper;
using System;
using System.Collections.Generic;

namespace Bingo.Dao.BingoDb.Dao.Impl
{
    public class ApplyInfoDao : DbBase, IApplyInfoDao
    {
        private readonly string SELECT_ApplyInfoEntity = "SELECT ApplyId,MomentId,MomentUId,UId,ApplyState,Source,CreateTime,UpdateTime FROM dbo.ApplyInfo ";

        protected override DbEnum GetDbEnum()
        {
            return DbEnum.BingoDb;
        }

        public List<ApplyInfoEntity> GetListByMomentId(Guid momentId)
        {
            var sql = SELECT_ApplyInfoEntity+ @" Where MomentId=@MomentId";
            using var Db = GetDbConnection();
            return Db.Query<ApplyInfoEntity>(sql,new { MomentId= momentId }).AsList();
        }

        public bool Insert(ApplyInfoEntity entity)
        {
            var sql = @"INSERT INTO dbo.Moment
                                  (ApplyId
                                  ,MomentId
                                  ,MomentUId
                                  ,UId
                                  ,ApplyState
                                  ,Source
                                  ,CreateTime
                                  ,UpdateTime)
                            VALUES
                                  (@ApplyId
                                  ,@MomentId
                                  ,@MomentUId
                                  ,@UId
                                  ,@ApplyState
                                  ,@Source
                                  ,@CreateTime
                                  ,@UpdateTime)";
            using var Db = GetDbConnection();
            return Db.Execute(sql, entity)>0;
        }
    }
}
