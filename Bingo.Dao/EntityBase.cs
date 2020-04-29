using System;

namespace Bingo.Dao
{
    public abstract class EntityBase
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 最新修改时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}
