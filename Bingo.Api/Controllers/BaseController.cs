using Bingo.Model.Base;
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
                //LogHelper.ErrorAsync(title, code.ToDescription(), ex);
            }

            return new JsonResult(new ResponseContext<object>(code, null));
        }

    }
}