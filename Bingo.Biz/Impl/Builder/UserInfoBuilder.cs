using Bingo.Dao.BingoDb.Entity;
using Bingo.Model.Base;
using Bingo.Model.Common;
using Bingo.Utils;
using Infrastructure;
using System;
using System.Collections.Generic;

namespace Bingo.Biz.Impl.Builder
{
    public static class UserInfoBuilder
    {
        public static UserInfoType BuildUserInfo(UserInfoEntity userInfo, RequestHead head,bool needDistance=true)
        {
            if (userInfo == null)
            {
                return null;
            }
            var result = new UserInfoType
            {
                UId = userInfo.UId,
                Gender = userInfo.Gender,
                Latitude=userInfo.Latitude,
                UserType=userInfo.UserType,
                Longitude=userInfo.Longitude,
                NickName=userInfo.NickName,
                Portrait=userInfo.Portrait,
                Signature=userInfo.Signature,
                IsRegister=userInfo.IsRegister,
                TagList=new List<TagItem>()
            };
            int index = 1;
            if (head.UId== userInfo.UId)
            {
                result.NickName = string.Format("{0}(我)", result.NickName);
            }
            if (userInfo.Gender == GenderEnum.Man)
            {
                result.GenderIcon = CommonConst.Man_GenderIcon;
                result.GenderColor = CommonConst.Color_Man_GenderIcon;
            }
            if (userInfo.Gender == GenderEnum.Woman)
            {
                result.GenderIcon = CommonConst.WoMan_GenderIcon;
                result.GenderColor = CommonConst.Color_WoMan_GenderIcon;
            }
            var distance = LocationHelper.GetDistanceDesc(userInfo.Latitude, userInfo.Longitude, head.Latitude, head.Longitude);
            if (!string.IsNullOrEmpty(distance)&& needDistance)
            {
                result.TagList.Add(new TagItem()
                {
                    Type = TagTypeEnum.LocationInfo,
                    Content = distance,
                    Index = index++
                });
            }
            if(userInfo.BirthDate.HasValue&& userInfo.BirthDate.Value> Convert.ToDateTime("1990-01-01"))
            {
                AddTag(result.TagList, TagTypeEnum.AgeGrade, userInfo.BirthDate.GetAgeYear(), index++);
                AddTag(result.TagList, TagTypeEnum.Constellation, userInfo.BirthDate.GetConstellation(), index++);
            }
            
            if (userInfo.SchoolName.IsNullOrEmpty())
            {
                AddTag(result.TagList, TagTypeEnum.Default, GetLocationInfo(userInfo), index++);
            }
            else
            {
                AddTag(result.TagList, TagTypeEnum.Default, userInfo.SchoolName, index++);
            }
            
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
            if (tagList.Count > 1)
            {
                if (content.Length > 10)
                {
                    content = content.Substring(0, 9) + "...";
                }
            }
            else
            {
                if (content.Length > 5)
                {
                    content = content.Substring(0, 4) + "...";
                }
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
