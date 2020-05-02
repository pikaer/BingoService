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
    public class ProductController : BaseController
    {
        private readonly IMomentListBiz mentListBiz = SingletonProvider<MomentListBiz>.Instance;
        private readonly IMomentActionBiz momentActionBiz = SingletonProvider<MomentActionBiz>.Instance;

        /// <summary>
        /// 动态列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult MomentList(RequestContext<MomentListRequest> request)
        {
            try
            {
                if (request == null)
                {
                    return ErrorJsonResult(ErrCodeEnum.ParametersIsNotValid_Code);
                }
                if (request.Head == null)
                {
                    return ErrorJsonResult(ErrCodeEnum.InvalidRequestHead);
                }
                if (request.Data == null || request.Data.PageIndex < 0||request.Head==null||request.Head.UId<=0)
                {
                    return ErrorJsonResult(ErrCodeEnum.InvalidRequestBody);
                }
                return new JsonResult(mentListBiz.MomentList(request));
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, "ProductController.MomentList", ex);
            }
        }

        [HttpPost]
        public JsonResult MomentDetail(RequestContext<MomentDetailRequest> request)
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
                if (request.Data == null || request.Data.MomentId == null)
                {
                    return ErrorJsonResult(ErrCodeEnum.InvalidRequestBody);
                }
                return new JsonResult(momentActionBiz.MomentDetail(request.Data.MomentId, request.Head.UId));
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, "ProductController.MomentDetail", ex);
            }
        }
    }
}