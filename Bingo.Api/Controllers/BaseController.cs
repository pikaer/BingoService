﻿using Bingo.Biz.Impl;
using Bingo.Biz.Interface;
using Bingo.Model.Base;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;

namespace Bingo.Api.Controllers
{
    /// <summary>
    /// 控制器基类
    /// </summary>
    public class BaseController : Controller
    {
        private readonly ILogBiz log = SingletonProvider<LogBiz>.Instance;
        
        /// <summary>
        /// 处理请求数据流
        /// </summary>
        /// <returns></returns>
        protected string GetInputString()
        {
            using (var streamReader = new StreamReader(Request.Body))
            {
                string json = streamReader.ReadToEnd();
                if (!string.IsNullOrEmpty(json))
                {
                    while (json.IndexOf("\\/", StringComparison.Ordinal) != -1)
                    {
                        json = json.Replace("\\/", "/");
                    }
                }

                return json;
            }
        }

        /// <summary>
        /// 获取错误的返回
        /// </summary>
        protected JsonResult ErrorJsonResult(ErrCodeEnum code, string title = null, Exception ex = null)
        {
            if (!string.IsNullOrEmpty(title) || ex != null)
            {
                log.ErrorAsync(title, ex);
            }

            return new JsonResult(new ResponseContext<object>(code, null));
        }

    }
}