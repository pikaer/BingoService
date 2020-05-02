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
                if (request.Data == null)
                {
                    return ErrorJsonResult(ErrCodeEnum.InvalidRequestBody);
                }
                return new JsonResult(momentActionBiz.Publish(request));
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, "PublishController.PublishMoment", ex);
            }
        }

        [HttpPost]
        public JsonResult MyPublishList(RequestContext<MyPublishListRequest> request)
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
                return new JsonResult(momentActionBiz.MyPublishList(request));
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, "PublishController.MyPublishList", ex);
            }
        }

        [HttpPost]
        public JsonResult MyPublishMomentDetail(RequestContext<MyPublishMomentDetailRequest> request)
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
                if (request.Data == null||request.Data.MomentId==null)
                {
                    return ErrorJsonResult(ErrCodeEnum.InvalidRequestBody);
                }
                return new JsonResult(momentActionBiz.MyPublishMomentDetail(request.Data.MomentId));
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, "PublishController.MyPublishMomentDetail", ex);
            }
        }

    }
}