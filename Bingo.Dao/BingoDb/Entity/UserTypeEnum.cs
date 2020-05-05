using System.ComponentModel;

namespace Bingo.Dao.BingoDb.Entity
{
    public enum UserTypeEnum
    {
        [Description("正常注册用户/默认")]
        Default = 0,

        [Description("客服")]
        ServiceUser = 1,

        [Description("模拟用户")]
        SimulationUser = 2,

        [Description("超级管理员")]
        Admin =3
    }
}
