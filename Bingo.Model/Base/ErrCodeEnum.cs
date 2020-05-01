﻿using System.ComponentModel;

namespace Bingo.Model.Base
{
    public enum ErrCodeEnum
    {
        [Description("操作成功")]
        Success = 0,

        [Description("操作失败")]
        Failure = 9000,

        [Description("无权限")]
        PermissionDenied = 9990,

        [Description("关联关系错误")]
        RelativeError = 9991,

        [Description("数据已存在")]
        DataIsExist = 9992,

        [Description("数据不存在")]
        DataIsnotExist = 9993,

        [Description("数据查询错误")]
        QueryError = 9994,

        [Description("数据插入错误")]
        InsertError = 9995,

        [Description("数据删除错误")]
        DeleteError = 9996,

        [Description("数据修改错误")]
        UpdateError = 9997,

        [Description("参数错误")]
        ParameterError = 9998,

        [Description("内部错误")]
        InnerError = 9999,

        [Description("Token已过期")]
        AppTokenIsOutOfTime = 20001,

        [Description("输入的参数不能为空")]
        ParametersIsNotAllowedEmpty_Code = 10010,

        [Description("无法解析字符串, 请确认是否为Json格式")]
        ParametersIsNotValid_Code = 10101,

        [Description("请求体不合法")]
        InvalidRequestBody = 10009,

        [Description("请求头不合法")]
        InvalidRequestHead = 10009,

        [Description("该用户不存在")]
        UserNoExist = 80001,

        [Description("用户金币余额不足")]
        CoinEmpty = 80002,

        [Description("内容不合法")]
        MessageCheckError = 80003,
        
    }
}
