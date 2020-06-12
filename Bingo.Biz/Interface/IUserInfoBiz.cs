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

        ResponseContext<LoginResponse> GetLoginInfoByCode(string code, PlatformEnum platform);

        ResponseContext<UserInfoType> GetUserInfo(RequestHead head,long uId);

        bool UpdateUserLocation(long uId, double latitude, double longitude);

        ResponseContext<UserInfoType> Register(RequestContext<RegisterRequest> request);

        Response UpdateUserInfo(RequestContext<UpdateUserInfoType> request);

        ResponseContext<UpdateUserInfoType> GetUserUpdateInfo(RequestHead head);
    }
}
