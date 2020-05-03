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
            var sql = SELECT_ApplyInfoEntity+ @" Where MomentId=@MomentId  order by UpdateTime desc ";
            using var Db = GetDbConnection();
            return Db.Query<ApplyInfoEntity>(sql,new { MomentId= momentId }).AsList();
        }

        public bool Insert(ApplyInfoEntity entity)
        {
            var sql = @"INSERT INTO dbo.ApplyInfo
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

        public ApplyInfoEntity GetByMomentIdAndUId(Guid momentId, long uId)
        {
            if (uId <= 0)
            {
                return null;
            }
            var sql = SELECT_ApplyInfoEntity + @" Where MomentId=@MomentId and UId=@UId";
            using var Db = GetDbConnection();
            return Db.QueryFirstOrDefault<ApplyInfoEntity>(sql, new { MomentId = momentId, UId= uId });
        }

        public bool UpdateState(ApplyStateEnum applyState, Guid applyId)
        {
            using var Db = GetDbConnection();
            string sql = @"UPDATE dbo.ApplyInfo
                               SET ApplyState = @ApplyState
                                  ,UpdateTime = @UpdateTime
                               WHERE ApplyId=@ApplyId";
            return Db.Execute(sql, new { UpdateTime = DateTime.Now, ApplyId = applyId, ApplyState= applyState }) > 0;
          
        }

        public ApplyInfoEntity GetByApplyId(Guid applyId)
        {
            var sql = SELECT_ApplyInfoEntity + @" Where ApplyId=@ApplyId";
            using var Db = GetDbConnection();
            return Db.QueryFirstOrDefault<ApplyInfoEntity>(sql, new { ApplyId = applyId});
        }

        public List<ApplyInfoEntity> GetListByMomentUId(long uId)
        {
            if (uId <= 0)
            {
                return null;
            }
            var sql = SELECT_ApplyInfoEntity + @" Where MomentUId=@MomentUId  order by UpdateTime desc ";
            using var Db = GetDbConnection();
            return Db.Query<ApplyInfoEntity>(sql, new { MomentUId = uId }).AsList();
        }

        public List<ApplyInfoEntity> GetListByUId(long uId)
        {
            if (uId <= 0)
            {
                return null;
            }
            var sql = SELECT_ApplyInfoEntity + @" Where UId=@UId  order by UpdateTime desc ";
            using var Db = GetDbConnection();
            return Db.Query<ApplyInfoEntity>(sql, new { UId = uId }).AsList();
        }
    }
}
