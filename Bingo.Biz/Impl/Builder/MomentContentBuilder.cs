﻿using Bingo.Dao.BingoDb.Entity;
using Bingo.Model.Common;
using Bingo.Utils;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bingo.Biz.Impl.Builder
{
    public static class MomentContentBuilder
    {
        public static List<ContentItem> BuilderContent(MomentEntity moment,bool displayContact=true)
        {
            var resultList = new List<ContentItem>();
            int index = 1;
            AddItem(resultList, index++, "发布时间", DateTimeHelper.GetDateDesc(moment.CreateTime, true));
            if (moment.StopTime.HasValue)
            {
                string stopStr = moment.StopTime.Value.ToString("yyyy-MM-dd HH:mm");
                if (IsOverTime(moment.StopTime))
                {
                    stopStr += "(已过期)";
                }
                AddItem(resultList, index++, "截止时间", stopStr);
            }
            AddItem(resultList, index++, "性别要求", GenderMap(moment.ExpectGender));
            AddItem(resultList, index++, "人数限制",string.Format("{0}人",moment.NeedCount));
            if (moment.IsOffLine)
            {
                AddItem(resultList, index++, "地点", moment.Place);
            }
            if (displayContact)
            {
                AddItem(resultList, index++, "联系方式", "通过申请后可查看", TagTypeEnum.Contact);
            }
            return resultList;
        }

        public static List<ContentItem> BuilderContent2Contact(MomentEntity moment, UserInfoEntity userInfo, bool displayContact)
        {
            var resultList = new List<ContentItem>();
            int index = 1;
            AddItem(resultList, index++, "发布时间", DateTimeHelper.GetDateDesc(moment.CreateTime, true), TagTypeEnum.PublishTime);
            if (moment.StopTime.HasValue)
            {
                string stopStr = moment.StopTime.Value.ToString(DateTimeHelper.yMdHm);
                if (IsOverTime(moment.StopTime))
                {
                    stopStr += "(已过期)";
                }
                AddItem(resultList, index++, "截止时间", stopStr);
            }
            AddItem(resultList, index++, "性别要求", GenderMap(moment.ExpectGender));
            AddItem(resultList, index++, "人数限制", string.Format("{0}人", moment.NeedCount));
            AddItem(resultList, index++, "地点", moment.Place);
            if (displayContact)
            {
                if (!string.IsNullOrEmpty(userInfo.Mobile))
                {
                    AddItem(resultList, index++, "手机号", userInfo.Mobile, TagTypeEnum.Contact);
                }
                if (!string.IsNullOrEmpty(userInfo.WeChatNo))
                {
                    AddItem(resultList, index++, "微信号", userInfo.WeChatNo, TagTypeEnum.Contact);
                }
                if (!string.IsNullOrEmpty(userInfo.QQNo))
                {
                    AddItem(resultList, index++, "QQ号", userInfo.QQNo, TagTypeEnum.Contact);
                }
            }
            return resultList;
        }

        public static string GetShareTitle(MomentEntity moment)
        {
            if (moment == null)
            {
                return null;
            }
            var stringBuilder = new StringBuilder();
            if (!string.IsNullOrEmpty(moment.Title))
            {
                stringBuilder.AppendFormat("{0}：",moment.Title);
            }
            if (!string.IsNullOrEmpty(moment.Content))
            {
                stringBuilder.Append(moment.Content);
            }
            return stringBuilder.ToString();
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


        public static string BtnTextMap(MomentStateEnum state, DateTime? stopTime,bool isApply, bool selfFlag, bool isOverCount)
        {
            if (isApply)
            {
                return "查看我的申请";
            }
            if (IsOverTime(stopTime) || isOverCount|| selfFlag)
            {
                return "";
            }
            if (state== MomentStateEnum.正常发布中)
            {
                return "申请参与";
            }

            return "";
        }

        public static string MomentStateMap(MomentStateEnum state, DateTime? stopTime, bool isOverCount)
        {
            if (IsOverTime(stopTime))
            {
                return "活动已过期";
            }
            if (isOverCount)
            {
                return "活动人数已满";
            }
            switch (state)
            {
                case MomentStateEnum.正常发布中:
                    return "活动进行中";
                case MomentStateEnum.审核中:
                case MomentStateEnum.被投诉审核中:
                    return "活动审核中";
                case MomentStateEnum.审核被拒绝:
                    return "活动审核不通过";
                case MomentStateEnum.被关小黑屋中:
                case MomentStateEnum.永久不支持上线:
                    return "被关小黑屋中";
                default:
                    return "活动已过期";
            }
        }

        public static string VerifyStateMap(MomentStateEnum state)
        {
            switch (state)
            {
                case MomentStateEnum.审核中:
                case MomentStateEnum.被投诉审核中:
                    return "活动审核中";
                case MomentStateEnum.审核被拒绝:
                    return "活动审核不通过";
                case MomentStateEnum.正常发布中:
                case MomentStateEnum.被关小黑屋中:
                case MomentStateEnum.永久不支持上线:
                default:
                    return "已审核通过";
            }
        }

        public static bool IsOverTime(DateTime? stopTime)
        {
            return stopTime.HasValue && stopTime.Value < DateTime.Now;
        }

        public static string TextColorMap(MomentStateEnum state, DateTime? stopTime, bool isOverCount)
        {
            if (IsOverTime(stopTime)|| isOverCount)
            {
                return CommonConst.Color_Black;
            }
            switch (state)
            {
                case MomentStateEnum.正常发布中:
                    return CommonConst.Color_Green;
                case MomentStateEnum.审核中:
                case MomentStateEnum.被投诉审核中:
                case MomentStateEnum.审核被拒绝:
                    return CommonConst.Color_Red;
                case MomentStateEnum.被关小黑屋中:
                case MomentStateEnum.永久不支持上线:
                default:
                    return CommonConst.Color_Black; 
            }
        }
    }
}
