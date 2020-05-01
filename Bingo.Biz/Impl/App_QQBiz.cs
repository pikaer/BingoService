using Bingo.Biz.Interface;
using Bingo.Model.DTO;
using Infrastructure;

namespace Bingo.Biz.Impl
{
    public class App_QQBiz : IAppBiz
    {
        public string GetOpenId(string loginCode)
        {
            string myAppid = JsonSettingHelper.AppSettings["BingoAppId_QQ"];
            string mySecret = JsonSettingHelper.AppSettings["BingoSecret_QQ"];
            string url = string.Format("https://api.q.qq.com/sns/jscode2session?appid={0}&secret={1}&js_code={2}&grant_type=authorization_code", myAppid, mySecret, loginCode);
            var dto = HttpHelper.HttpGet<OpenIdDTO>(url);
            return dto?.OpenId;
        }

        public string GetAccessToken()
        {
            string myAppid = JsonSettingHelper.AppSettings["BingoAppId_QQ"];
            string mySecret = JsonSettingHelper.AppSettings["BingoSecret_QQ"];
            string url = string.Format("https://api.q.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", myAppid, mySecret);
            var token = HttpHelper.HttpGet<AccessTokenDTO>(url);
            if (token == null || token.Errcode != 0)
            {
                return null;
            }
            return token.Access_token;
        }

    }
}
