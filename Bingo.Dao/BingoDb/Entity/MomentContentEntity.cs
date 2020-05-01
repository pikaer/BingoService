using System;

namespace Bingo.Dao.BingoDb.Entity
{
    public class MomentContentEntity: EntityBase
    {
        public Guid MomentContentId { get; set; }

        /// <summary>
        /// 动态Id
        /// </summary>
        public Guid MomentId { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public TagTypeEnum TagType { get; set; }
    }
}
