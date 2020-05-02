using System;

namespace Bingo.Dao.BingoDb.Entity
{
    public class ApplyInfoEntity : EntityBase
    {
        /// <summary>
        /// 申请Id
        /// </summary>
        public Guid ApplyId { get; set; }

        /// <summary>
        /// 动态Id
        /// </summary>
        public Guid MomentId { get; set; }

        /// <summary>
        /// 动态发布用户Id（减少连表查询）
        /// </summary>
        public long MomentUId { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public long UId { get; set; }

        /// <summary>
        /// 申请状态
        /// </summary>
        public ApplyStateEnum ApplyState { get; set; }

        /// <summary>
        /// 申请来源
        /// </summary>
        public string Source { get; set; }
    }
}
