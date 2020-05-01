using Bingo.Dao.BingoDb.Entity;

namespace Bingo.Model.Common
{
    public class TagItem
    {
        /// <summary>
        /// 类别
        /// </summary>
        public TagTypeEnum Type { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 排序值
        /// </summary>
        public int Index { get; set; }
    }
}
