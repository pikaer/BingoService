using Bingo.Biz;
using Bingo.Model.Base;
using Bingo.Model.Contract;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System;

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
    }
}