using Bingo.Dao.BingoDb.Entity;
using Dapper;
using System;

namespace Bingo.Dao.BingoDb.Dao.Impl
{
    public class UserInfoDao : DbBase,IUserInfoDao 
    {
        protected override DbEnum GetDbEnum()
        {
            return DbEnum.BingoDb;
        }

        public UserInfoEntity GetUserInfoByOpenId(string openId)
        {
            throw new NotImplementedException();
        }

        public UserInfoEntity GetUserInfoByUid(long uid)
        {
            throw new NotImplementedException();
        }

        public long InsertUserInfo(UserInfoEntity userInfo)
        {
            using (var Db = GetDbConnection())
            {
                var sql = @"INSERT INTO dbo.bingo_UserInfo
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
                return Db.QueryFirst<long>(sql, userInfo);
            }
        }

        
    }
}
