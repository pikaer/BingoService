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
    public class ApplyController : BaseController
    {
        private readonly IApplyBiz applyBiz = SingletonProvider<ApplyBiz>.Instance;

        [HttpPost]
        public JsonResult ApplyMomentList(RequestContext<ApplyMomentListRequest> request)
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
                return new JsonResult(applyBiz.ApplyMomentList(request.Head.UId));
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, "ApplyController.ApplyMomentList", ex);
            }
        }

        [HttpPost]
        public JsonResult ApplyMomentDetail(RequestContext<AskMomentDetailRequest> request)
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
                if (request.Data == null)
                {
                    return ErrorJsonResult(ErrCodeEnum.InvalidRequestBody);
                }
                return new JsonResult(applyBiz.ApplyMomentDetail(request.Data.ApplyId, request.Head.UId));
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, "ApplyController.ApplyMomentDetail", ex);
            }
        }
    }
}