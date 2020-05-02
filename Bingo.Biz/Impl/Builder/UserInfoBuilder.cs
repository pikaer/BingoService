﻿using Bingo.Dao.BingoDb.Entity;
using Bingo.Model.Common;
using Infrastructure;
using System.Collections.Generic;

namespace Bingo.Biz.Impl.Builder
{
    public static class UserInfoBuilder
    {
        public static UserInfoType BuildUserInfo(UserInfoEntity userInfo, MomentEntity moment=null)
        {
            if (userInfo == null)
            {
                return null;
            }
            var result = new UserInfoType
            {
                UId = userInfo.UId,
                Gender = userInfo.Gender,
                NickName=userInfo.NickName,
                Portrait=userInfo.Portrait,
                Signature=userInfo.Signature,
                IsRegister=userInfo.IsRegister,
                TagList=new List<TagItem>()
            };
            int index = 1;
            if (moment != null)
            {
                result.IsHide = moment.IsHide;
                if (moment.IsHide)
                {
                    result.NickName = moment.HidingNickName;
                    AddTag(result.TagList, TagTypeEnum.Default, "匿名发布", index++);
                }
            }
            AddTag(result.TagList, TagTypeEnum.Default, userInfo.BirthDate.GetAgeYear(), index++);
            AddTag(result.TagList, TagTypeEnum.Default, userInfo.BirthDate.GetConstellation(), index++);
            AddTag(result.TagList,TagTypeEnum.Default,userInfo.SchoolName,index++);
            AddTag(result.TagList, TagTypeEnum.Default, GetLocationInfo(userInfo), index++);
            return result;
        }

        private static string GetLocationInfo(UserInfoEntity userInfo)
        {
            if (userInfo == null)
            {
                return string.Empty;
            }
            string text = "";
            if (!string.IsNullOrEmpty(userInfo.Province) && !string.IsNullOrEmpty(userInfo.City))
            {
                text=string.Format("{0} {1}", userInfo.Province, userInfo.City);
            }
            else if(string.IsNullOrEmpty(userInfo.Province) && !string.IsNullOrEmpty(userInfo.City))
            {
                text = userInfo.City;
            }
            else if (string.IsNullOrEmpty(userInfo.City) && !string.IsNullOrEmpty(userInfo.Province))
            {
                text = userInfo.Province;
            }
            if (text.Length > 8)
            {
                return text.Substring(0, 7)+"...";
            }
            return text;
        }


        private static void AddTag(List<TagItem>tagList,TagTypeEnum type,string content, int index, string icon=null)
        {
            if (string.IsNullOrEmpty(content))
            {
                return;
            }
            tagList.Add(new TagItem()
            {
                Type = type,
                Content = content,
                Icon = icon,
                Index=index
            });
        }
    }
}
