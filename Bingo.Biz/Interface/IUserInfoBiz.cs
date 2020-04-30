using Bingo.Dao.BingoDb.Entity;

namespace Bingo.Biz.Interface
{
    public interface IUserInfoBiz
    {
        UserInfoEntity GetUserInfoByUid(long uid);

        UserInfoEntity GetUserInfoByOpenId(string openId);

        int InsertUserInfo(UserInfoEntity userInfo);
    }
}
