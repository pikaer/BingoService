using Bingo.Biz.Impl.Builder;
using Bingo.Biz.Interface;
using Bingo.Dao.BingoDb.Entity;
using Bingo.Model.Base;
using Bingo.Model.DTO;
using Bingo.Utils;
using Infrastructure;
using System;
using System.Collections.Generic;

namespace Bingo.Biz.Impl
{
    public class App_WechatBiz : IAppBiz
    {
        private readonly IUserInfoBiz uerInfoBiz = SingletonProvider<UserInfoBiz>.Instance;

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

        public bool MsgSecCheck(string message)
        {

            var token = GetAccessToken();
            if (token == null || token.IsNullOrEmpty())
            {
                return true;
            }
            string url = string.Format("https://api.weixin.qq.com/wxa/msg_sec_check?access_token={0}", token);

            var request = new WeChatMsgSecCheckRequestDTO()
            {
                content = message
            };
            var response = HttpHelper.HttpPost<WeChatMsgSecCheckRequestDTO, WeChatResponseDTO>(url, request, 5);
            return response != null && response.Errcode == 0;
        }

        public bool Send_Activity_Join_Msg(MomentEntity moment, long targetUserId, bool joinSuccess,string joinMsg)
        {
            var targetUserInfo = uerInfoBiz.GetUserInfoByUid(targetUserId);
            if (targetUserInfo == null || moment == null)
            {
                return false;
            }
            string title = string.Format("{0}：{1}", moment.Title, moment.Content);
            string place = moment.IsOffLine ? moment.Place : "线上活动";
            string state = joinSuccess?"申请加入成功":"申请加入失败";
            var message = new WeChatMessageContext<ActivityJoinMsgDTO>()
            {
                touser = targetUserInfo.OpenId,
                template_id = CommonConst.Activity_Join_TmplId_WeChat,
                page = string.Format(CommonConst.BingoSharePageUrl,""),
                data = new ActivityJoinMsgDTO()
                {
                    thing2 = new Value(title),
                    thing3 = new Value(place),
                    phrase1 = new Value(state),
                    thing9 = new Value(joinMsg)
                }
            };

            var token = GetAccessToken();
            if (string.IsNullOrEmpty(token))
            {
                return false;
            }

            string url = string.Format(CommonConst.Message_Send_Url_WeChat, token);

            var response = HttpHelper.HttpPost<WeChatMessageContext<ActivityJoinMsgDTO>, WeChatResponseDTO>(url, message, 5);
            return response != null && response.Errcode == 0;
        }

        public bool Send_Activity_Cancel_Msg(MomentEntity moment,List<long>userIds)
        {
            if(userIds.IsNullOrEmpty()|| moment == null)
            {
                return false;
            }
            var token = GetAccessToken();
            if (string.IsNullOrEmpty(token))
            {
                return false;
            }

            string url = string.Format(CommonConst.Message_Send_Url_WeChat, token);

            foreach (long uid in userIds)
            {
                var targetUserInfo = uerInfoBiz.GetUserInfoByUid(uid);
                var momentUserInfo= uerInfoBiz.GetUserInfoByUid(moment.UId);
                if (targetUserInfo == null|| momentUserInfo==null)
                {
                    continue;
                }
                var message = new WeChatMessageContext<ActivityCancelMsgDTO>()
                {
                    touser = targetUserInfo.OpenId,
                    template_id = CommonConst.Activity_Cancel_TmplId_WeChat,
                    page = string.Format(CommonConst.BingoSharePageUrl, ""),
                    data = new ActivityCancelMsgDTO()
                    {
                        thing1 = new Value(string.Format("{0}：{1}", moment.Title, moment.Content)),
                        date2 = new Value(moment.CreateTime.ToString("f")),
                        name3 = new Value(momentUserInfo.NickName),
                        thing4 = new Value("活动取消，点击查看详情")
                    }
                };
                var response = HttpHelper.HttpPost<WeChatMessageContext<ActivityCancelMsgDTO>, WeChatResponseDTO>(url, message, 5);
            }
            return true;
        }

        public bool Send_Moment_Publish_Msg(MomentEntity moment, bool publishSuccess, string msg)
        {
            var targetUserInfo = uerInfoBiz.GetUserInfoByUid(moment.UId);
            if (targetUserInfo == null || moment == null)
            {
                return false;
            }
            string title = string.Format("{0}：{1}", moment.Title, moment.Content);
            string place = moment.IsOffLine ? moment.Place : "线上活动";
            string state = publishSuccess ? "动态审核通过" : "动态审核不通过";
            var message = new WeChatMessageContext<MomentPublishMsgDTO>()
            {
                touser = targetUserInfo.OpenId,
                template_id = CommonConst.Moment_Publish_TmplId_WeChat,
                page = string.Format(CommonConst.BingoSharePageUrl, ""),
                data = new MomentPublishMsgDTO()
                {
                    thing2 = new Value(title),
                    thing7 = new Value(place),
                    phrase5 = new Value(state),
                    thing8 = new Value(msg),
                    date4=new Value(DateTime.Now.ToString("f"))
                }
            };

            var token = GetAccessToken();
            if (string.IsNullOrEmpty(token))
            {
                return false;
            }

            string url = string.Format(CommonConst.Message_Send_Url_WeChat, token);

            var response = HttpHelper.HttpPost<WeChatMessageContext<MomentPublishMsgDTO>, WeChatResponseDTO>(url, message, 5);
            return response != null && response.Errcode == 0;
        }

        public bool Send_Moment_Join_Msg(MomentEntity moment, long targetUserId)
        {
            var targetUserInfo = uerInfoBiz.GetUserInfoByUid(targetUserId);
            if (targetUserInfo == null|| moment==null)
            {
                return false;
            }
            var message = new WeChatMessageContext<MomentJoinMsgDTO>()
            {
                touser = targetUserInfo.OpenId,
                template_id = CommonConst.Moment_Join_TmplId_WeChat,
                page = string.Format(CommonConst.BingoSharePageUrl, ""),
                data = new MomentJoinMsgDTO()
                {
                    thing1 = new Value(string.Format("{0}：{1}", moment.Title, moment.Content)),
                    thing2 = new Value(targetUserInfo.NickName),
                    thing3 = new Value("申请参与该活动，请审批")
                }
            };

            var token = GetAccessToken();
            if (string.IsNullOrEmpty(token))
            {
                return false;
            }

            string url = string.Format(CommonConst.Message_Send_Url_WeChat, token);

            var response = HttpHelper.HttpPost<WeChatMessageContext<MomentJoinMsgDTO>, WeChatResponseDTO>(url, message, 5);
            return response != null && response.Errcode == 0;
        }
    }
}
