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
    public class AskController : BaseController
    {
        private readonly IAskBiz askBiz = SingletonProvider<AskBiz>.Instance;

        [HttpPost]
        public JsonResult AskActivity(RequestContext<AskActivityRequest> request)
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
                if (request.Data == null)
                {
                    return ErrorJsonResult(ErrCodeEnum.InvalidRequestBody);
                }
                return new JsonResult(askBiz.Ask(request));
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, head, "AskController.AskActivity", ex);
            }
        }


        [HttpPost]
        public JsonResult AskAction(RequestContext<AskActionRequest> request)
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
                if (request.Data == null||string.IsNullOrEmpty(request.Data.Action))
                {
                    return ErrorJsonResult(ErrCodeEnum.InvalidRequestBody);
                }
                return new JsonResult(askBiz.AskAction(request));
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, head, "AskController.AskAction", ex);
            }
        }


        [HttpPost]
        public JsonResult AskMomentDetail(RequestContext<AskMomentDetailRequest> request)
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
                if (request.Data == null)
                {
                    return ErrorJsonResult(ErrCodeEnum.InvalidRequestBody);
                }
                return new JsonResult(askBiz.AskMomentDetail(request.Data.ApplyId,request.Head));
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, head, "AskController.AskMomentDetail", ex);
            }
        }
    }
}