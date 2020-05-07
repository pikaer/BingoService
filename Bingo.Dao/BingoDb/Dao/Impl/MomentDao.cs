using Bingo.Dao.BingoDb.Entity;
using System;
using Dapper;
using System.Collections.Generic;

namespace Bingo.Dao.BingoDb.Dao.Impl
{
    public class MomentDao : DbBase, IMomentDao
    {
        private readonly string SELECT_MomentEntity = "SELECT * FROM dbo.Moment ";
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

        public List<MomentEntity> GetMomentListByState(MomentStateEnum state)
        {
            var sql = @"SELECT top (20) * FROM dbo.Moment Where State=@State  and IsDelete=0 and StopTime>GETDATE() order by CreateTime ";
            using var Db = GetDbConnection();
            return Db.Query<MomentEntity>(sql, new { State = state }).AsList();
        }

        public int PendingCount()
        {
            var sql = @"SELECT count (1)  FROM dbo.Moment Where State=101  and IsDelete=0 and StopTime>GETDATE()";
            using var Db = GetDbConnection();
            return Db.QueryFirst<int>(sql);
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
                                  ,Address
                                  ,Latitude
                                  ,Longitude
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
                                  ,@Address
                                  ,@Latitude
                                  ,@Longitude
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
            var sql = SELECT_MomentEntity+ " WHERE IsDelete=0 and State=0 and NeedCount>ApplyCount and StopTime>GETDATE() order by CreateTime desc";
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

        public bool UpdateMoment(MomentEntity entity)
        {
            var sql = @"UPDATE dbo.Moment
                        SET IsOffLine = @IsOffLine,
                            IsHide = @IsHide,
                            HidingNickName = @HidingNickName,
                            NeedCount = @NeedCount,
                            StopTime = @StopTime,
                            Place = @Place,
                            ExpectGender = @ExpectGender,
                            ShareType = @ShareType,
                            Latitude = @Latitude,
                            Longitude = @Longitude,
                            Title = @Title,
                            Content = @Content,
                            State = @State,
                            UpdateTime = @UpdateTime
                        WHERE MomentId=@MomentId";
            using var Db = GetDbConnection();
            return Db.Execute(sql, entity) > 0;
        }

        public bool UpdateState(Guid momentId, MomentStateEnum momentState)
        {
            var sql = @"UPDATE dbo.Moment
                        SET State =@State,
                            UpdateTime = @UpdateTime
                        WHERE MomentId=@MomentId";
            using var Db = GetDbConnection();
            return Db.Execute(sql, new
            {
                MomentId = momentId,
                State = momentState,
                UpdateTime = DateTime.Now
            }) > 0;
        }
    }
}
