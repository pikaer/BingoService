﻿using Bingo.Dao.BingoDb.Entity;
using Bingo.Model.Common;
using Infrastructure;
using System.Collections.Generic;

namespace Bingo.Biz.Impl.Builder
{
    public static class UserInfoBuilder
    {
        public static UserInfoType BuildUserInfo(UserInfoEntity userInfo)
        {
            var result = new UserInfoType
            {
                UId = userInfo.UId,
                Gender = userInfo.Gender,
                NickName=userInfo.NickName,
                Portrait=userInfo.Portrait,
                TagList=new List<TagItem>()
            };
            int index = 1;
            AddTag(result.TagList, TagTypeEnum.Default, userInfo.BirthDate.GetAgeYear(), index++);
            AddTag(result.TagList, TagTypeEnum.Default, userInfo.BirthDate.GetConstellation(), index++);
            AddTag(result.TagList,TagTypeEnum.Default,userInfo.SchoolName,index++);
            return result;
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
