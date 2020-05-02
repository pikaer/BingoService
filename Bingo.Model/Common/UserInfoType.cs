using Bingo.Dao.BingoDb.Entity;
using System.Collections.Generic;

namespace Bingo.Model.Common
{
    public class UserInfoType
    {
        public long UId { get; set; }

        public string NickName { get; set; }

        /// <summary>
        /// 是否已经注册账户
        /// </summary>
        public bool IsRegister { get; set; }

        /// <summary>
        /// 是否隐身
        /// </summary>
        public bool IsHide { get; set; }

        /// <summary>
        /// 头像路径
        /// </summary>
        public string Portrait { get; set; }

        /// <summary>
        /// 用户的性别，值为1时是男性，值为2时是女性，值为0时是未知,此时一般是用户未授权
        /// </summary>
        public GenderEnum Gender { get; set; }


        public List<TagItem> TagList { get; set; }
    }
}
