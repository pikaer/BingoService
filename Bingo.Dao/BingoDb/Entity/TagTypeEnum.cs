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
        Contact =200,

        [Description("星座")]
        Constellation = 300,

        [Description("如：90后")]
        AgeGrade = 400,

        [Description("活动主题")]
        Title = 500,

        [Description("活动说明")]
        Content = 600,


        [Description("发布时间")]
        PublishTime = 700,
    }
}
