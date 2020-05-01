using Bingo.Biz.Interface;
using Bingo.Model.DTO;
using Infrastructure;

namespace Bingo.Biz.Impl
{
    public class App_WechatBiz : IAppBiz
    {
        public string GetOpenId(string loginCode)
        {
            string myAppid = JsonSettingHelper.AppSettings["BingoAppId_WeChat"];
            string mySecret = JsonSettingHelper.AppSettings["BingoSecret_WeChat"];
            string url = string.Format("https://api.weixin.qq.com/sns/jscode2session?appid={0}&secret={1}&js_code={2}&grant_type=authorization_code", myAppid, mySecret, loginCode);

            var dto=HttpHelper.HttpGet<OpenIdDTO>(url);
            return dto?.OpenId;
        }

        /// <summary>
        /// 获取小程序全局唯一后台接口调用凭据
        /// </summary>
        public string GetAccessToken()
        {
            string myAppid = JsonSettingHelper.AppSettings["BingoAppId_WeChat"];
            string mySecret = JsonSettingHelper.AppSettings["BingoSecret_WeChat"];
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", myAppid, mySecret);

            var token = HttpHelper.HttpGet<AccessTokenDTO>(url);
            if (token == null || token.Errcode != 0)
            {
                return null;
            }
            return token.Access_token;
        }
    }
}
