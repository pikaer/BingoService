using System.ComponentModel;

namespace Bingo.Model.Common
{
    public enum TagTypeEnum
    {
        [Description("默认")]
        Default = 0,

        [Description("用户定位")]
        LocationInfo = 101
    }
}
