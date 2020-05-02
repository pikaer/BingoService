using Bingo.Dao.BingoDb.Entity;
using System;
using Dapper;
using System.Collections.Generic;

namespace Bingo.Dao.BingoDb.Dao.Impl
{
    public class MomentDao : DbBase, IMomentDao
    {
        private readonly string SELECT_MomentEntity = "SELECT MomentId,UId,IsDelete,IsOffLine,IsHide,HidingNickName,State,NeedCount,StopTime,Place,ExpectGender,ShareType,Content,CreateTime,UpdateTime FROM dbo.Moment ";
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
                                  ,NeedCount
                                  ,StopTime
                                  ,Place
                                  ,ExpectGender
                                  ,ShareType
                                  ,Content
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
                                  ,@NeedCount
                                  ,@StopTime
                                  ,@Place
                                  ,@ExpectGender
                                  ,@ShareType
                                  ,@Content
                                  ,@CreateTime
                                  ,@UpdateTime)";
            using var Db = GetDbConnection();
            return Db.Execute(sql, entity);
        }

        public List<MomentEntity> GetMomentListByParam()
        {
            var sql = SELECT_MomentEntity;
            using var Db = GetDbConnection();
            return Db.Query<MomentEntity>(sql).AsList();
        }
    }
}
