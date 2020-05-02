using Infrastructure;

namespace Bingo.Model.Base
{
    public class ResponseContext<T>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ResponseContext()
        {
            ResultCode = ErrCodeEnum.Success;
            ResultMessage = ErrCodeEnum.Success.ToDescription();
            Data = default;
        }

        public ResponseContext(ErrCodeEnum codeEnum)
        {
            ResultCode = codeEnum;
            ResultMessage = codeEnum.ToDescription();
            Data = default;
        }

        public ResponseContext(T data)
        {
            ResultCode = ErrCodeEnum.Success;
            ResultMessage = ErrCodeEnum.Success.ToDescription();
            Data = data;
        }

        public ResponseContext(ErrCodeEnum codeEnum, T data, string msg = null)
        {
            ResultCode = codeEnum;
            ResultMessage = codeEnum.ToDescription();
            Data = data;
            if (!msg.IsNullOrEmpty())
            {
                ResultMessage = msg;
            }
        }

        /// <summary>
        /// 错误码
        /// </summary>
        public ErrCodeEnum ResultCode { get; set; }

        /// <summary>
        /// 返回文本
        /// </summary>
        public string ResultMessage { get; set; }

        /// <summary>
        /// 响应体
        /// </summary>
        public T Data { get; set; }
    }

    public class Response
    {
        /// <summary>
        /// 错误码
        /// </summary>
        public ErrCodeEnum ResultCode { get; set; }

        /// <summary>
        /// 返回文本
        /// </summary>
        public string ResultMessage { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public Response()
        {
            ResultCode = ErrCodeEnum.Success;
            ResultMessage = ErrCodeEnum.Success.ToDescription();
        }

        public Response(ErrCodeEnum err, string msg = null)
        {
            ResultCode = err;
            ResultMessage = err.ToDescription();
            if (!msg.IsNullOrEmpty())
            {
                ResultMessage = msg;
            }
        }
    }

}
