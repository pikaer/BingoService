using System.ComponentModel;

namespace Bingo.Model.Common
{
    public enum ContentTypeEnum
    {
        [Description("默认")]
        Default = 0,

        [Description("联系方式，支持复制")]
        ContactInfo = 101,

        [Description("用户地址信息")]
        AdressInfo =102

    }
}
