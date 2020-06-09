using Bingo.Biz.Interface;
using Bingo.Dao.BingoDb.Entity;
using Bingo.Model.Base;
using Bingo.Model.DTO;
using Bingo.Utils;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bingo.Biz.Impl
{
    public class App_WechatBiz : IAppBiz
    {
        private readonly ILogBiz log = SingletonProvider<LogBiz>.Instance;
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

        public void Send_Activity_Join_MsgAsync(MomentEntity moment, long targetUserId, bool joinSuccess,string joinMsg)
        {
            var targetUserInfo = uerInfoBiz.GetUserInfoByUid(targetUserId);
            var token = GetAccessToken();
            if (targetUserInfo == null || moment == null|| string.IsNullOrEmpty(token))
            {
                return;
            }
            
            string title = string.Format("{0}：{1}", moment.Title, moment.Content);
            string place = moment.IsOffLine ? moment.Place : "线上活动";
            string state = joinSuccess?"加入成功":"加入失败";
            var message = new WeChatMessageContext<ActivityJoinMsgDTO>()
            {
                touser = targetUserInfo.OpenId,
                access_token = token,
                template_id = CommonConst.Activity_Join_TmplId_WeChat,
                page = string.Format(CommonConst.BingoSharePageUrl, moment.MomentId.ToString()),
                data = new ActivityJoinMsgDTO()
                {
                    thing2 = new Value(title),
                    thing3 = new Value(place),
                    phrase1 = new Value(state),
                    thing9 = new Value(joinMsg)
                }
            };

            
            string url = string.Format(CommonConst.Message_Send_Url_WeChat, token);

            HttpHelper.HttpPost<WeChatMessageContext<ActivityJoinMsgDTO>, WeChatResponseDTO>(url, message, 5);
        }

        public void Send_Activity_Cancel_MsgAsync(MomentEntity moment,List<long>userIds)
        {
            if(userIds.IsNullOrEmpty()|| moment == null)
            {
                return;
            }
            var token = GetAccessToken();
            if (string.IsNullOrEmpty(token))
            {
                return;
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
                    access_token = token,
                    template_id = CommonConst.Activity_Cancel_TmplId_WeChat,
                    page = string.Format(CommonConst.BingoSharePageUrl, moment.MomentId.ToString()),
                    data = new ActivityCancelMsgDTO()
                    {
                        thing1 = new Value(string.Format("{0}：{1}", moment.Title, moment.Content)),
                        date2 = new Value(moment.CreateTime.ToString("yyyy年MM月dd日 HH:mm")),
                        name3 = new Value(momentUserInfo.NickName),
                        thing4 = new Value("活动取消，点击查看详情")
                    }
                };
                HttpHelper.HttpPost<WeChatMessageContext<ActivityCancelMsgDTO>, WeChatResponseDTO>(url, message, 5);
            }
        }

        public void Send_Moment_Publish_MsgAsync(MomentEntity moment, string targetUser, string remark)
        {
            try
            {
                Task.Factory.StartNew(() =>
                {
                    var token = GetAccessToken();
                    if (string.IsNullOrEmpty(token))
                    {
                        return;
                    }

                    string title = string.Format("{0}：{1}", moment.Title, moment.Content);
                    string place = moment.IsOffLine ? moment.Place : "线上活动";
                    string state = moment.State == MomentStateEnum.正常发布中 ? "审核通过" : "审核不通过";
                    var message = new WeChatMessageContext<MomentPublishMsgDTO>()
                    {
                        touser = targetUser,
                        access_token= token,
                        template_id = CommonConst.Moment_Publish_TmplId_WeChat,
                        page = string.Format(CommonConst.BingoSharePageUrl, moment.MomentId.ToString()),
                        data = new MomentPublishMsgDTO()
                        {
                            thing2 = new Value(title),
                            thing7 = new Value(place),
                            phrase5 = new Value(state),
                            thing8 = new Value(remark),
                            date4 = new Value(DateTime.Now.ToString("yyyy年MM月dd日 HH:mm"))
                        }
                    };

                    string url = string.Format(CommonConst.Message_Send_Url_WeChat, token);

                    HttpHelper.HttpPost<WeChatMessageContext<MomentPublishMsgDTO>, WeChatResponseDTO>(url, message, 5);
                });
            }
            catch(Exception ex)
            {
                log.Error("Send_Moment_Publish_MsgAsync",ex);
            }
        }

        public void Send_Moment_Join_MsgAsync(MomentEntity moment, long targetUserId,string momentUserOpenId)
        {
            var targetUserInfo = uerInfoBiz.GetUserInfoByUid(targetUserId);
            if (targetUserInfo == null|| moment==null)
            {
                return;
            }
            var token = GetAccessToken();
            var message = new WeChatMessageContext<MomentJoinMsgDTO>()
            {
                touser = momentUserOpenId,
                access_token = token,
                template_id = CommonConst.Moment_Join_TmplId_WeChat,
                page = string.Format(CommonConst.BingoSharePageUrl, moment.MomentId.ToString()),
                data = new MomentJoinMsgDTO()
                {
                    thing1 = new Value(string.Format("{0}：{1}", moment.Title, moment.Content)),
                    thing2 = new Value(targetUserInfo.NickName),
                    thing3 = new Value("申请参与该活动，请审批")
                }
            };

            if (string.IsNullOrEmpty(token))
            {
                return;
            }

            string url = string.Format(CommonConst.Message_Send_Url_WeChat, token);

            HttpHelper.HttpPost<WeChatMessageContext<MomentJoinMsgDTO>, WeChatResponseDTO>(url, message, 5);
        }
    }
}
