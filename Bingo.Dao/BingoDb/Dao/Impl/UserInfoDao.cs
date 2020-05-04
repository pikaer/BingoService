using Bingo.Dao.BingoDb.Entity;
using Dapper;
using System;

namespace Bingo.Dao.BingoDb.Dao.Impl
{
    public class UserInfoDao : DbBase,IUserInfoDao 
    {
        private readonly string SELECT_UserInfoEntity = "SELECT UId,OpenId,Platform,Gender,LiveState,Grade,UserType,NickName,BirthDate,Area,Province,City,Country,Mobile,WeChatNo,QQNo,Portrait,Signature,SchoolName,EntranceDate,IsDelete,IsRegister,Latitude,Longitude,LastLoginTime,CreateTime,UpdateTime FROM dbo.UserInfo ";


        protected override DbEnum GetDbEnum()
        {
            return DbEnum.BingoDb;
        }

        public UserInfoEntity GetUserInfoByOpenId(string openId)
        {
            var sql = SELECT_UserInfoEntity + @" Where OpenId=@OpenId ";
            using var Db = GetDbConnection();
            return Db.QueryFirstOrDefault<UserInfoEntity>(sql, new
            {
                OpenId=openId
            });
        }

        public UserInfoEntity GetUserInfoByUid(long uid)
        {
            var sql = SELECT_UserInfoEntity + @" Where UId=@UId ";
            using var Db = GetDbConnection();
            return Db.QueryFirstOrDefault<UserInfoEntity>(sql, new
            {
                UId = uid
            });
        }

        public long InsertUserInfo(UserInfoEntity userInfo)
        {
            var sql = @"INSERT INTO dbo.UserInfo
                                  (OpenId
                                  ,Platform
                                  ,LastLoginTime
                                  ,CreateTime
                                  ,UpdateTime)
                            VALUES
                                  (@OpenId
                                  ,@Platform
                                  ,@LastLoginTime
                                  ,@CreateTime
                                  ,@UpdateTime)";
            sql += "SELECT CAST(SCOPE_IDENTITY() as bigint)";
            using var Db = GetDbConnection();
            return Db.QueryFirst<long>(sql, userInfo);
        }

        public bool UpdateUserLocation(long uId, double latitude, double longitude)
        {
            var sql = @"UPDATE dbo.UserInfo
                        SET Latitude =@Latitude,
                            Longitude = @Longitude,
                            UpdateTime = @UpdateTime
                        WHERE Uid=@Uid";
            using var Db = GetDbConnection();
            return Db.Execute(sql, new
            {
                Uid=uId,
                Latitude= latitude,
                Longitude= longitude,
                UpdateTime=DateTime.Now
            })>0;
        }

        public bool Register(long uId, GenderEnum gender, string nickName, string avatarUrl, string country, string province, string city)
        {
            var sql = @"UPDATE dbo.UserInfo
                        SET Gender =@Gender,
                            NickName = @NickName,
                            Portrait = @Portrait,
                            Country = @Country,
                            Province = @Province,
                            City = @City,
                            IsRegister =1,
                            UpdateTime = @UpdateTime
                        WHERE Uid=@Uid";
            using var Db = GetDbConnection();
            return Db.Execute(sql, new
            {
                Uid = uId,
                Gender = gender,
                NickName = nickName,
                Portrait = avatarUrl,
                Country = country,
                Province = province,
                City = city,
                UpdateTime = DateTime.Now
            }) > 0;
        }

        public bool UpdateUserInfo(UserInfoEntity userInfo)
        {
            var sql = @"UPDATE dbo.UserInfo
                        SET NickName =@NickName,
                            Gender = @Gender,
                            LiveState = @LiveState,
                            Grade = @Grade,
                            SchoolName = @SchoolName,
                            BirthDate = @BirthDate,
                            Mobile =@Mobile,
                            WeChatNo =@WeChatNo,
                            QQNo =@QQNo,
                            UpdateTime = @UpdateTime
                        WHERE UId=@UId";
            using var Db = GetDbConnection();
            return Db.Execute(sql, userInfo) > 0;
        }
    }
}
