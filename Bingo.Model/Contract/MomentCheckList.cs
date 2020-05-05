using Bingo.Dao.BingoDb.Entity;

namespace Bingo.Model.Contract
{
    public class MomentCheckList
    {
        /// <summary>
        /// 页码（分页传递数据）
        /// </summary>
        public int PageIndex { get; set; }

        public MomentStateEnum State { get; set; }
    }
}
