using Bingo.Dao.BingoDb.Entity;
using Bingo.Model.Base;
using Bingo.Model.Common;
using Bingo.Utils;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bingo.Biz.Impl.Builder
{
    public static class UserInfoBuilder
    {
        public static UserInfoType BuildUserInfo(UserInfoEntity userInfo, RequestHead head)
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
            if (!string.IsNullOrEmpty(distance))
            {
                result.TagList.Add(new TagItem()
                {
                    Type = TagTypeEnum.LocationInfo,
                    Content = distance,
                    Index = 1
                });
            }
            if (!userInfo.SchoolName.IsNullOrEmpty())
            {
                AddTag(result.TagList, TagTypeEnum.Default, userInfo.SchoolName, 3);
            }

            if (userInfo.BirthDate.HasValue&& userInfo.BirthDate.Value> Convert.ToDateTime("1990-01-01"))
            {
                AddTag(result.TagList, TagTypeEnum.AgeGrade, userInfo.BirthDate.GetAgeYear(), 2);
                AddTag(result.TagList, TagTypeEnum.Constellation, userInfo.BirthDate.GetConstellation(), 4);
            }

            AddTag(result.TagList, TagTypeEnum.Default, GetLocationInfo(userInfo), 5);

            if (result.TagList.Count > 3)
            {
                result.TagList = result.TagList.OrderBy(a=>a.Index).Take(3).ToList();
            }
            return result;
        }

        public static UserInfoType BuildUserInfoV1(UserInfoEntity userInfo, RequestHead head)
        {
            if (userInfo == null)
            {
                return null;
            }
            var result = new UserInfoType
            {
                UId = userInfo.UId,
                Gender = userInfo.Gender,
                Latitude = userInfo.Latitude,
                UserType = userInfo.UserType,
                Longitude = userInfo.Longitude,
                NickName = userInfo.NickName,
                Portrait = userInfo.Portrait,
                Signature = userInfo.Signature,
                IsRegister = userInfo.IsRegister,
                TagList = BuildAllTags(userInfo)
            };
            if (head.UId == userInfo.UId)
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
           
            return result;
        }


        private static List<TagItem> BuildAllTags(UserInfoEntity userInfo)
        {
            var tagList = new List<TagItem>();
            if (userInfo.BirthDate.HasValue && userInfo.BirthDate.Value > Convert.ToDateTime("1990-01-01"))
            {
                AddTag(tagList, TagTypeEnum.AgeGrade, userInfo.BirthDate.GetAgeYear(), 1);
                AddTag(tagList, TagTypeEnum.Constellation, userInfo.BirthDate.GetConstellation(), 2);
            }

            if (!userInfo.SchoolName.IsNullOrEmpty())
            {
                AddTag(tagList, TagTypeEnum.Default, userInfo.SchoolName, 3);
            }

            AddTag(tagList, TagTypeEnum.Default, GetLocationInfo(userInfo), 4);
            return tagList;
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
                return text.CutText(10);
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
