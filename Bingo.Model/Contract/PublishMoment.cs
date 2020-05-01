using Bingo.Dao.BingoDb.Entity;
using Bingo.Model.Base;
using System.Collections.Generic;

namespace Bingo.Model.Contract
{
    public class PublishMomentRequest
    {
        /// <summary>
        /// 是否线下活动，默认false
        /// </summary>
        public bool IsOffLine { get; set; }

        /// <summary>
        /// 是否隐身
        /// </summary>
        public bool IsHide { get; set; }

        /// <summary>
        /// 匿名时候对应的昵称
        /// </summary>
        public string HidingNickName { get; set; }

        public List<MomentContentType> ContentList { get; set; }
    }

    public class MomentContentType
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public TagTypeEnum TagType { get; set; }
    }
}
