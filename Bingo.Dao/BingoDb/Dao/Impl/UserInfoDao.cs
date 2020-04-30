using Bingo.Dao.BingoDb.Entity;
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

        public int InsertUserInfo(UserInfoEntity userInfo)
        {
            throw new NotImplementedException();
        }

        
    }
}
