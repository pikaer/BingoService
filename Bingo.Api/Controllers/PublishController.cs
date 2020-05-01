using Bingo.Biz.Impl;
using Bingo.Biz.Interface;
using Bingo.Model.Base;
using Bingo.Model.Contract;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Bingo.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PublishController : BaseController
    {
        private readonly IMomentActionBiz momentActionBiz = SingletonProvider<MomentActionBiz>.Instance;

        /// <summary>
        /// 文本内容安全检测
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PublishMoment(RequestContext<PublishMomentRequest> request)
        {
            try
            {
                if (request == null)
                {
                    return ErrorJsonResult(ErrCodeEnum.ParametersIsNotValid_Code);
                }
                if (!HeadCheck(request.Head))
                {
                    return ErrorJsonResult(ErrCodeEnum.InvalidRequestHead);
                }
                if (request.Data == null || request.Data.ContentList.IsNullOrEmpty())
                {
                    return ErrorJsonResult(ErrCodeEnum.InvalidRequestBody);
                }
                var response = new Response();
                bool success = momentActionBiz.Publish(request);
                if (!success)
                {
                    response.Code = ErrCodeEnum.MessageCheckError;
                    response.ResultMessage = ErrCodeEnum.MessageCheckError.ToDescription();
                }
                else
                {
                    response.ResultMessage = "发布成功";
                }
                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, "PublishController.PublishMoment", ex);
            }
        }
    }
}