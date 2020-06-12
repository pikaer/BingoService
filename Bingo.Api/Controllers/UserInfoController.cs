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
            RequestHead head = default;
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
                head = request.Head;
                if (request.Data == null || request.Data.Code.IsNullOrEmpty() || request.Head == null)
                {
                    return ErrorJsonResult(ErrCodeEnum.InvalidRequestBody);
                }
                return new JsonResult(userInfoBiz.GetLoginInfoByCode(request.Data.Code, request.Head.Platform));
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, head, "UserInfoController.Login", ex);
            }
        }

        [HttpPost]
        public JsonResult GetUserInfo(RequestContext<GetUserInfoRequest> request)
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
                return new JsonResult(userInfoBiz.GetUserInfo(request.Head,request.Data.UId));
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, head,"UserInfoController.GetUserInfo", ex);
            }
        }


        [HttpPost]
        public JsonResult UpdateUserLocation(RequestContext<UpdateUserLocationRequest> request)
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
                var response = new Response();
                var success = userInfoBiz.UpdateUserLocation(request.Head.UId, request.Data.Latitude, request.Data.Longitude);
                if (success)
                {
                    response.ResultCode = ErrCodeEnum.Success;
                    response.ResultMessage = "更新成功";
                }
                else
                {
                    response.ResultCode = ErrCodeEnum.Failure;
                    response.ResultMessage = "更新失败";
                }
                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, head, "UserInfoController.UpdateUserLocation", ex);
            }
        }


        [HttpPost]
        public JsonResult Register(RequestContext<RegisterRequest> request)
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
                return new JsonResult(userInfoBiz.Register(request));
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, head,"UserInfoController.Register", ex);
            }
        }


        [HttpPost]
        public JsonResult GetUserUpdateInfo(RequestContext<object> request)
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
                return new JsonResult(userInfoBiz.GetUserUpdateInfo(request.Head));
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, head, "UserInfoController.GetUserUpdateInfo", ex);
            }
        }


        [HttpPost]
        public JsonResult UpdateUserInfo(RequestContext<UpdateUserInfoType> request)
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
                return new JsonResult(userInfoBiz.UpdateUserInfo(request));
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ErrCodeEnum.InnerError, head, "UserInfoController.UpdateUserInfo", ex);
            }
        }

    }
}