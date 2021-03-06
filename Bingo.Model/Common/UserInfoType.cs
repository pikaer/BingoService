﻿using Bingo.Dao.BingoDb.Entity;
using System.Collections.Generic;

namespace Bingo.Model.Common
{
    public class UserInfoType
    {
        public long UId { get; set; }

        /// <summary>
        /// 小程序端-用户唯一标示
        /// </summary>
        public string OpenId { get; set; }

        public string NickName { get; set; }

        /// <summary>
        /// 用户注册渠道
        /// </summary>
        public PlatformEnum Platform { get; set; }

        /// <summary>
        /// 是否已经注册账户
        /// </summary>
        public bool IsRegister { get; set; }

        /// <summary>
        /// 头像路径
        /// </summary>
        public string Portrait { get; set; }

        public string GenderIcon { get; set; }

        public string GenderColor { get; set; }

        /// <summary>
        /// 用户的性别，值为1时是男性，值为2时是女性，值为0时是未知,此时一般是用户未授权
        /// </summary>
        public GenderEnum Gender { get; set; }

        /// <summary>
        /// 用户类别
        /// </summary>
        public UserTypeEnum UserType { get; set; }

        /// <summary>
        /// 纬度，范围为 -90~90，负数表示南纬
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// 经度，范围为 -180~180，负数表示西经
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// 个性签名
        /// </summary>
        public string Signature { get; set; }

        public List<TagItem> TagList { get; set; }
    }
}
