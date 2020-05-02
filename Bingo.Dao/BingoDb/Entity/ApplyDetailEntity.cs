using System;

namespace Bingo.Dao.BingoDb.Entity
{
    public class ApplyDetailEntity : EntityBase
    {
        /// <summary>
        /// 申请详情Id
        /// </summary>
        public Guid ApplyDetailId { get; set; }

        /// <summary>
        /// 申请Id
        /// </summary>
        public Guid ApplyId { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public long UId { get; set; }

        /// <summary>
        /// 申请内容
        /// </summary>
        public string Content { get; set; }
    }
}
