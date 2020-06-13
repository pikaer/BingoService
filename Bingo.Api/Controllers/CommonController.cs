using Bingo.Biz;
using Bingo.Dao.BingoDb.Entity;
using Bingo.Model.Base;
using Bingo.Model.Contract;
using Bingo.Utils;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Bingo.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    public class CommonController : BaseController
    {
        /// <summary>
        /// 文本内容安全检测
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult MsgSecCheck(RequestContext<MsgSecCheckRequest> request)
        {
            RequestHead head = default;
            try
            {
                if (request == null)
                {
                    return ErrorJsonResult(ErrCodeEnum.ParametersIsNotValid_Code);
                }
                if (!CheckAuth(request.Head))
                {
                    return ErrorJsonResult(ErrCodeEnum.InvalidRequestHead);
                }
                head = request.Head;
                if (request.Data == null || request.Data.TextContent.IsNullOrEmpty())
                {
                    return ErrorJsonResult(ErrCodeEnum.InvalidRequestBody);
                }
                var response = new Response();
                bool msgOk=AppFactory.Factory(request.Head.Platform).MsgSecCheck(request.Data.TextContent);
                if (!msgOk)
                {
                    response.ResultCode = ErrCodeEnum.MessageCheckError;
                    response.ResultMessage = ErrCodeEnum.MessageCheckError.ToDescription();
                }
                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, head, "CommonController.MsgSecCheck", ex);
            }
        }

        /// <summary>
        ///  微信小程序模板校验签名
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string VerifySignature(string signature, string timestamp, string nonce, string echostr)
        {
            bool success = CheckSignature(signature, timestamp, nonce);
            return success ? echostr : "校验失败";
        }

        /// <summary>
        /// 验证微信签名
        /// </summary>
        private bool CheckSignature(string signature, string timestamp, string nonce)
        {
            try
            {
                var token = CommonConst.BingoToken;

                string[] arrTmp = { token, timestamp, nonce };

                Array.Sort(arrTmp);

                string tmpStr = string.Join("", arrTmp);

                var sha1 = new SHA1CryptoServiceProvider();
                var data = sha1.ComputeHash(Encoding.UTF8.GetBytes(tmpStr));

                var sb = new StringBuilder();
                foreach (var t in data)
                {
                    sb.Append(t.ToString("X2"));
                }

                sha1.Dispose();

                return sb.ToString().ToLower().Equals(signature);
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        [HttpGet]
        public JsonResult Test()
        {
            AppFactory.Factory(PlatformEnum.QQ_MiniApp).MsgSecCheck("特3456书yuuo莞6543李zxcz蒜7782法fgnv级");
            return new JsonResult("SUSSESS");
        }
    }
}