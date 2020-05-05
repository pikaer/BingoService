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
    public class AdminController : BaseController
    {
        private readonly IAdminBiz adminBiz = SingletonProvider<AdminBiz>.Instance;

        [HttpPost]
        public JsonResult MomentCheckList(RequestContext<MomentCheckList> request)
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
                return new JsonResult(adminBiz.MomentCheckList(request));
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, head, "AdminController.MomentCheckList", ex);
            }
        }

        [HttpPost]
        public JsonResult MomentCheckDetail(RequestContext<MomentCheckDetailRequest> request)
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
                return new JsonResult(adminBiz.MomentCheckDetail(request));
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, head, "AdminController.MomentCheckDetail", ex);
            }
        }

        [HttpPost]
        public JsonResult ServiceDetail(RequestContext<object> request)
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
                return new JsonResult(adminBiz.ServiceDetail(request.Head));
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, head, "AdminController.ServiceDetail", ex);
            }
        }

    }
}