using Bingo.Dao.BingoDb.Entity;

namespace Bingo.Dao.BingoDb.Dao
{
    public interface IUserInfoDao
    {
        UserInfoEntity GetUserInfoByUid(long uid);

        UserInfoEntity GetUserInfoByOpenId(string openId);

        int InsertUserInfo(UserInfoEntity userInfo);
    }
}
