using Bingo.Dao.BingoDb.Entity;

namespace Bingo.Model.Common
{
    public class ContentItem
    {
        /// <summary>
        /// 类别
        /// </summary>
        public TagTypeEnum Type { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }

        public int Index { get; set; }
    }
}
