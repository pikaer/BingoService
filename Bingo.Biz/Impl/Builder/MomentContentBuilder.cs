using Bingo.Dao.BingoDb.Entity;
using Bingo.Model.Common;
using Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace Bingo.Biz.Impl.Builder
{
    public static class MomentContentBuilder
    {
        public static List<ContentItem> BuilderContent(List<MomentContentEntity> contentList)
        {
            if (contentList.IsNullOrEmpty())
            {
                return null;
            }
            return contentList.Select(a => new ContentItem()
            {
                Type=a.TagType,
                Content = string.Format("{0}:{1}", a.Title, a.Content)
            }).ToList();
        }
    }
}
