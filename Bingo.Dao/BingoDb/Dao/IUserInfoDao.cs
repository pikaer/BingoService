using Bingo.Dao.BingoDb.Entity;

namespace Bingo.Dao.BingoDb.Dao
{
    public interface IUserInfoDao
    {
        UserInfoEntity GetUserInfoByUid(long uid);

        UserInfoEntity GetUserInfoByOpenId(string openId);

        long InsertUserInfo(UserInfoEntity userInfo);

        bool UpdateUserLocation(long uId, double latitude, double longitude);

        bool UpdateMobile(long uId, string mobile);

        bool UpdateWeChatNo(long uId, string weChatNo);

        bool UpdateQQNo(long uId, string qqNo);

        bool Register(long uId, GenderEnum gender, string nickName, string avatarUrl,string country,string province,string city);

        bool UpdateUserInfo(UserInfoEntity userInfo);
    }
}
