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
        public JsonResult Login(RequestContext<LoginRequest> request)
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
                if (request.Data == null || request.Data.Code.IsNullOrEmpty() || request.Head == null)
                {
                    return ErrorJsonResult(ErrCodeEnum.InvalidRequestBody);
                }
                var response = new ResponseContext<LoginResponse>()
                {
                    Data = new LoginResponse()
                    {
                        UId = userInfoBiz.GetUIdByCode(request.Data.Code, request.Head.Platform)
                    }
                };
                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, "UserInfoController.Login", ex);
            }
        }

        [HttpPost]
        public JsonResult GetUserInfo(RequestContext<GetUserInfoRequest> request)
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
                return new JsonResult(userInfoBiz.GetUserInfo(request.Head.UId));
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, "UserInfoController.GetUserInfo", ex);
            }
        }


        [HttpPost]
        public JsonResult UpdateUserLocation(RequestContext<UpdateUserLocationRequest> request)
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
                var response = new Response();
                var success = userInfoBiz.UpdateUserLocation(request.Head.UId, request.Data.Latitude, request.Data.Longitude);
                if (success)
                {
                    response.Code = ErrCodeEnum.Success;
                    response.ResultMessage = "更新成功";
                }
                else
                {
                    response.Code = ErrCodeEnum.Failure;
                    response.ResultMessage = "更新失败";
                }
                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, "UserInfoController.UpdateUserLocation", ex);
            }
        }


        [HttpPost]
        public JsonResult Register(RequestContext<RegisterRequest> request)
        {
            try
            {
                if (request == null)
                {
                    return ErrorJsonResult(ErrCodeEnum.ParametersIsNotValid_Code);
                }
                if (request.Data == null)
                {
                    return ErrorJsonResult(ErrCodeEnum.InvalidRequestBody);
                }
                if (!HeadCheck(request.Head))
                {
                    return ErrorJsonResult(ErrCodeEnum.InvalidRequestHead);
                }
                return new JsonResult(userInfoBiz.Register(request));
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, "UserInfoController.Register", ex);
            }
        }
    }
}