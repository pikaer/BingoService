namespace Bingo.Dao.BingoDb.Entity
{
    public enum MomentStateEnum
    {
        正常发布中=0,
        审核中=101,
        审核被拒绝=102,
        被投诉审核中=103,
        被关小黑屋中=104,
        永久不支持上线=105
    }
}
