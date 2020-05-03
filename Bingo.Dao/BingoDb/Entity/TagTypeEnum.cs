using System.ComponentModel;

namespace Bingo.Dao.BingoDb.Entity
{
    public enum TagTypeEnum
    {
        [Description("默认")]
        Default = 0,

        [Description("用户定位")]
        LocationInfo = 101,

        [Description("联系方式")]
        Contact =200
    }
}
