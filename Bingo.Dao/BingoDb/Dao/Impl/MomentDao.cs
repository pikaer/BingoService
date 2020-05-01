using Bingo.Dao.BingoDb.Entity;
using System;
using Dapper;
using System.Collections.Generic;

namespace Bingo.Dao.BingoDb.Dao.Impl
{
    public class MomentDao : DbBase, IMomentDao
    {
        protected override DbEnum GetDbEnum()
        {
            return DbEnum.BingoDb;
        }

        public MomentEntity GetMomentByMomentId(Guid momentId)
        {
            throw new NotImplementedException();
        }

        public List<MomentEntity> GetMomentListByUid(long uid)
        {
            throw new NotImplementedException();
        }

        public int Insert(MomentEntity entity)
        {
            var sql = @"INSERT INTO dbo.Moment
                                  (MomentId
                                  ,UId
                                  ,IsDelete
                                  ,IsOffLine
                                  ,IsHide
                                  ,HidingNickName
                                  ,State
                                  ,CreateTime
                                  ,UpdateTime)
                            VALUES
                                  (@MomentId
                                  ,@UId
                                  ,@IsDelete
                                  ,@IsOffLine
                                  ,@IsHide
                                  ,@HidingNickName
                                  ,@State
                                  ,@CreateTime
                                  ,@UpdateTime)";
            using var Db = GetDbConnection();
            return Db.Execute(sql, entity);
        }

        public List<MomentEntity> GetMomentListByParam()
        {
            throw new NotImplementedException();
        }
    }
}
