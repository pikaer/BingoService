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

        /// <summary>
        /// 动态列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult MomentList()
        {
            try
            {
                string json = GetInputString();
                if (string.IsNullOrEmpty(json))
                {
                    return ErrorJsonResult(ErrCodeEnum.ParametersIsNotAllowedEmpty_Code);
                }
                var request = json.JsonToObject<RequestContext<MomentListRequest>>();
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
                return ErrorJsonResult(ErrCodeEnum.InnerError, "MomentList", ex);
            }
        }
    }
}