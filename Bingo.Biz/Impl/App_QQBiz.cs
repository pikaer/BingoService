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
    public class App_QQBiz : IAppBiz
    {
        private readonly ILogBiz log = SingletonProvider<LogBiz>.Instance;
        private readonly IUserInfoBiz uerInfoBiz = SingletonProvider<UserInfoBiz>.Instance;

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
            string url = string.Format("https://api.q.qq.com/api/getToken?grant_type=client_credential&appid={0}&secret={1}", myAppid, mySecret);
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
            string url = string.Format("https://api.q.qq.com/api/json/security/MsgSecCheck?access_token={0}", token);

            var request = new MsgSecCheckRequestDTO()
            {
                access_token = token,
                appid = JsonSettingHelper.AppSettings["BingoAppId_QQ"],
                content = message
            };
            var response = HttpHelper.HttpPost<MsgSecCheckRequestDTO, WeChatResponseDTO>(url, request, 5);
            return response != null && response.Errcode == 0;
        }

        public void Send_Activity_Join_MsgAsync(MomentEntity moment, long targetUserId, bool joinSuccess, string joinMsg)
        {
            try
            {
                Task.Factory.StartNew(() =>
                {
                    var targetUserInfo = uerInfoBiz.GetUserInfoByUid(targetUserId);
                    var token = GetAccessToken();
                    if (targetUserInfo == null || moment == null || string.IsNullOrEmpty(token))
                    {
                        return;
                    }

                    string title = string.Format("{0}：{1}", moment.Title, moment.Content);
                    string place = moment.IsOffLine ? moment.Place : "线上活动";
                    string state = joinSuccess ? "加入成功" : "加入失败";
                    var message = new MessageContext<Dictionary<string, Value>>()
                    {
                        touser = targetUserInfo.OpenId,
                        access_token = token,
                        template_id = CommonConst.Activity_Join_TmplId_QQ,
                        page = string.Format(CommonConst.BingoSharePageUrl, moment.MomentId.ToString()),
                        data = new Dictionary<string, Value>()
                        {
                            {"keyword1", new Value(title.CutText(20))},
                            {"keyword2", new Value(place)},
                            {"keyword3", new Value(state)},
                            {"keyword4", new Value(joinMsg.CutText(20))},
                            {"keyword5", new Value(DateTime.Now.ToString("yyyy年MM月dd日 HH:mm"))}
                        }
                    };

                    string url = string.Format(CommonConst.Message_Send_Url_QQ, token);

                    HttpHelper.HttpPost<MessageContext<Dictionary<string, Value>>, WeChatResponseDTO>(url, message, 5);
                });
            }
            catch (Exception ex)
            {
                log.Error("Send_Moment_Publish_MsgAsync", ex);
            }
        }

        public void Send_Activity_Cancel_MsgAsync(MomentEntity moment, List<long> userIds)
        {
            return;
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
                    string state = moment.State == MomentStateEnum.正常发布中 ? "审核通过" : "审核不通过";
                    var message = new MessageContext<Dictionary<string, Value>>()
                    {
                        touser = targetUser,
                        access_token = token,
                        template_id = CommonConst.Moment_Publish_TmplId_QQ,
                        page = string.Format(CommonConst.BingoSharePageUrl, moment.MomentId.ToString()),
                        data = new Dictionary<string, Value>()
                        {
                            {"keyword1", new Value(title.CutText(20))},
                            {"keyword2", new Value(moment.CreateTime.ToString("yyyy年MM月dd日 HH:mm"))},
                            {"keyword3", new Value(state)},
                            {"keyword4", new Value(remark.CutText(20))}
                        }
                    };

                    string url = string.Format(CommonConst.Message_Send_Url_QQ, token);

                    HttpHelper.HttpPost<MessageContext<Dictionary<string, Value>>, WeChatResponseDTO>(url, message, 5);
                });
            }
            catch (Exception ex)
            {
                log.Error("Send_Moment_Publish_MsgAsync", ex);
            }
        }

        public void Send_Moment_Join_MsgAsync(MomentEntity moment, long targetUserId, string momentUserOpenId)
        {
            try
            {
                Task.Factory.StartNew(() =>
                {
                    var targetUserInfo = uerInfoBiz.GetUserInfoByUid(targetUserId);
                    if (targetUserInfo == null || moment == null)
                    {
                        return;
                    }
                    var token = GetAccessToken();
                    if (string.IsNullOrEmpty(token))
                    {
                        return;
                    }
                    var message = new MessageContext<Dictionary<string, Value>>()
                    {
                        touser = momentUserOpenId,
                        access_token = token,
                        template_id = CommonConst.Moment_Join_TmplId_QQ,
                        page = string.Format(CommonConst.BingoSharePageUrl, moment.MomentId.ToString()),
                        data = new Dictionary<string, Value>()
                        {
                            {"keyword1", new Value(string.Format("{0}：{1}", moment.Title, moment.Content).CutText(20))},
                            {"keyword2", new Value(targetUserInfo.NickName.CutText(20))},
                            {"keyword3", new Value(DateTime.Now.ToString("yyyy年MM月dd日 HH:mm"))},
                            {"keyword4", new Value("申请参与该活动，请审批")}
                        }
                    };

                    string url = string.Format(CommonConst.Message_Send_Url_QQ, token);

                    HttpHelper.HttpPost<MessageContext<Dictionary<string, Value>>, WeChatResponseDTO>(url, message, 5);
                });
            }
            catch (Exception ex)
            {
                log.Error("Send_Moment_Publish_MsgAsync", ex);
            }
        }
    }
}
