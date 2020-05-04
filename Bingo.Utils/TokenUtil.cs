using Bingo.Model.Base;
using Infrastructure;

namespace Bingo.Utils
{
    public class TokenUtil
    {
        public static string GenerateToken(string platform,long uId)
        {
            string tokenText = string.Format("Token_{0}_Platform_{1}_UId_{2}", CommonConst.BingoToken, platform, uId);
            return Md5Helper.GetMd5Str32(tokenText);
        }
    }
}
