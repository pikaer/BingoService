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
    public class UserInfoController : BaseController
    {
        private readonly IUserInfoBiz userInfoBiz = SingletonProvider<UserInfoBiz>.Instance;

        [HttpPost]
        public JsonResult Login()
        {
            try
            {
                string json = GetInputString();
                if (string.IsNullOrEmpty(json))
                {
                    return ErrorJsonResult(ErrCodeEnum.ParametersIsNotAllowedEmpty_Code);
                }
                var request = json.JsonToObject<RequestContext<LoginRequest>>();
                if (request == null)
                {
                    return ErrorJsonResult(ErrCodeEnum.ParametersIsNotValid_Code);
                }
                if (request.Head == null)
                {
                    return ErrorJsonResult(ErrCodeEnum.InvalidRequestHead);
                }
                if (request.Data == null || request.Data.Code.IsNullOrEmpty()|| request.Head == null)
                {
                    return ErrorJsonResult(ErrCodeEnum.InvalidRequestBody);
                }
                var response = new LoginResponse()
                {
                    UId = userInfoBiz.GetUIdByCode(request.Data.Code, request.Head.Platform)
                };
                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, "UserInfoController.Login", ex);
            }
        }
    }
}