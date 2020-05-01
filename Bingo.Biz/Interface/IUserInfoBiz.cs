using Bingo.Dao.BingoDb.Entity;
using Bingo.Model.Base;
using Bingo.Model.Common;
using Bingo.Model.Contract;

namespace Bingo.Biz.Interface
{
    public interface IUserInfoBiz
    {
        UserInfoEntity GetUserInfoByUid(long uid);

        UserInfoEntity GetUserInfoByOpenId(string openId);

        long GetUIdByCode(string code, PlatformEnum platform);

        ResponseContext<UserInfoType> GetUserInfo(long uid);

        bool UpdateUserLocation(long uId, double latitude, double longitude);

        ResponseContext<UserInfoType> Register(RequestContext<RegisterRequest> request);
    }
}
