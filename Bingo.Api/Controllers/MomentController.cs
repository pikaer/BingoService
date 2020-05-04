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
    public class MomentController : BaseController
    {
        private readonly IMomentBiz momentBiz = SingletonProvider<MomentBiz>.Instance;

        [HttpPost]
        public JsonResult PublishMoment(RequestContext<PublishMomentRequest> request)
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
                return new JsonResult(momentBiz.Publish(request));
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, head, "MomentController.PublishMoment", ex);
            }
        }

        [HttpPost]
        public JsonResult MyPublishList(RequestContext<MyPublishListRequest> request)
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
                return new JsonResult(momentBiz.MyPublishList(request));
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, head, "MomentController.MyPublishList", ex);
            }
        }


        [HttpPost]
        public JsonResult MyPublishMomentDetail(RequestContext<MyPublishMomentDetailRequest> request)
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
                return new JsonResult(momentBiz.MyPublishMomentDetail(request.Data.MomentId, head));
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, head, "MomentController.MyPublishMomentDetailRequest", ex);
            }
        }

        [HttpPost]
        public JsonResult MomentAction(RequestContext<MomentActionRequest> request)
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
                return new JsonResult(momentBiz.MomentAction(request.Data.MomentId, request.Data.Action));
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, head, "MomentController.MomentAction", ex);
            }
        }

    }
}