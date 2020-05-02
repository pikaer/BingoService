using Bingo.Dao.BingoDb.Entity;
using Bingo.Model.Common;
using Infrastructure;
using System.Collections.Generic;

namespace Bingo.Biz.Impl.Builder
{
    public static class MomentContentBuilder
    {
        public static List<ContentItem> BuilderContent(MomentEntity moment)
        {
            var resultList = new List<ContentItem>();
            int index = 1;
            AddItem(resultList, index++, "发布时间", DateTimeHelper.GetDateDesc(moment.CreateTime, true));
            AddItem(resultList, index++, "性别要求", GenderMap(moment.ExpectGender));
            AddItem(resultList, index++, "人数限制",string.Format("{0}人",moment.NeedCount));
            AddItem(resultList, index++, "地点", moment.Place);
            if (moment.StopTime.HasValue)
            {
                AddItem(resultList, index++, "截止时间", moment.StopTime.Value.ToString("yyyy-MM-dd HH:mm"));
            }
            AddItem(resultList, index++, "活动主题", moment.Title);
            AddItem(resultList, index++, "说明", moment.Content);
            return resultList;
        }

        private static void AddItem(List<ContentItem> resultList,int index,string title, string content, TagTypeEnum type= TagTypeEnum.Default,string icon=null)
        {
            if (string.IsNullOrEmpty(content))
            {
                return;
            }
            resultList.Add(new ContentItem()
            {
                Type = type,
                Title = "",
                Content = string.Format("{0}：{1}", title, content),
                Icon = icon,
                Index = index
            });
        }

        private static string GenderMap(GenderEnum gender)
        {
            switch (gender)
            {
                case GenderEnum.Man:
                    return "只要男生";
                case GenderEnum.Woman:
                    return "只要女生";
                case GenderEnum.Default:
                case GenderEnum.All:
                default:
                    return "男女不限";
            }
        }

        public static string MomentStateMap(MomentStateEnum state)
        {
            switch (state)
            {
                case MomentStateEnum.正常发布中:
                    return "进行中";
                case MomentStateEnum.审核中:
                case MomentStateEnum.被投诉审核中:
                    return "审核中";
                case MomentStateEnum.审核被拒绝:
                    return "审核不通过";
                case MomentStateEnum.被关小黑屋中:
                case MomentStateEnum.永久不支持上线:
                    return "被关小黑屋中";
                default:
                    return "进行中";
            }
        }

        public static TextColorEnum TextColorMap(MomentStateEnum state)
        {
            switch (state)
            {
                case MomentStateEnum.正常发布中:
                    return TextColorEnum.Green;
                case MomentStateEnum.审核中:
                case MomentStateEnum.被投诉审核中:
                case MomentStateEnum.审核被拒绝:
                    return TextColorEnum.Red;
                case MomentStateEnum.被关小黑屋中:
                case MomentStateEnum.永久不支持上线:
                default:
                    return TextColorEnum.Default; 
            }
        }
    }
}
