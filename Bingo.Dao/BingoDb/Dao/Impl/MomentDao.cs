using Bingo.Dao.BingoDb.Entity;
using System;
using Dapper;
using System.Collections.Generic;

namespace Bingo.Dao.BingoDb.Dao.Impl
{
    public class MomentDao : DbBase, IMomentDao
    {
        private readonly string SELECT_MomentEntity = "SELECT MomentId,UId,IsDelete,IsOffLine,IsHide,HidingNickName,State,NeedCount,ApplyCount,StopTime,Place,ExpectGender,ShareType,Title,Content,CreateTime,UpdateTime FROM dbo.Moment ";
        protected override DbEnum GetDbEnum()
        {
            return DbEnum.BingoDb;
        }

        public MomentEntity GetMomentByMomentId(Guid momentId)
        {
            var sql = SELECT_MomentEntity + @" Where MomentId=@MomentId ";
            using var Db = GetDbConnection();
            return Db.QueryFirstOrDefault<MomentEntity>(sql, new { MomentId = momentId });
        }

        public List<MomentEntity> GetMomentListByUid(long uid)
        {
            var sql = SELECT_MomentEntity+ @" Where UId=@UId  and IsDelete=0  order by CreateTime desc";
            using var Db = GetDbConnection();
            return Db.Query<MomentEntity>(sql,new { UId= uid }).AsList();
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
                                  ,ApplyCount
                                  ,StopTime
                                  ,Place
                                  ,ExpectGender
                                  ,ShareType
                                  ,Title
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
                                  ,@ApplyCount
                                  ,@StopTime
                                  ,@Place
                                  ,@ExpectGender
                                  ,@ShareType
                                  ,@Title
                                  ,@Content
                                  ,@CreateTime
                                  ,@UpdateTime)";
            using var Db = GetDbConnection();
            return Db.Execute(sql, entity);
        }

        public List<MomentEntity> GetMomentListByParam()
        {
            var sql = SELECT_MomentEntity+ " WHERE IsDelete=0 and NeedCount>ApplyCount  order by CreateTime desc";
            using var Db = GetDbConnection();
            return Db.Query<MomentEntity>(sql).AsList();
        }

        public bool UpdateStopTime(Guid momentId)
        {
            var sql = @"UPDATE dbo.Moment
                        SET StopTime = @UpdateTime,
                            UpdateTime = @UpdateTime
                        WHERE MomentId=@MomentId";
            using var Db = GetDbConnection();
            return Db.Execute(sql, new
            {
                MomentId = momentId,
                UpdateTime = DateTime.Now
            }) > 0;
        }

        public bool UpdateApplyCount(Guid momentId)
        {
            var sql = @"UPDATE dbo.Moment
                        SET ApplyCount = ApplyCount+1 ,
                            UpdateTime = @UpdateTime
                        WHERE MomentId=@MomentId";
            using var Db = GetDbConnection();
            return Db.Execute(sql, new
            {
                MomentId = momentId,
                UpdateTime = DateTime.Now
            }) > 0;
        }

        public bool Delete(Guid momentId)
        {
            var sql = @"UPDATE dbo.Moment
                        SET IsDelete =1,
                            UpdateTime = @UpdateTime
                        WHERE MomentId=@MomentId";
            using var Db = GetDbConnection();
            return Db.Execute(sql, new
            {
                MomentId = momentId,
                UpdateTime = DateTime.Now
            }) > 0;
        }
    }
}
