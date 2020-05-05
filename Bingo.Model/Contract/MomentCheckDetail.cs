using Bingo.Dao.BingoDb.Entity;
using Bingo.Model.Common;
using System;
using System.Collections.Generic;

namespace Bingo.Model.Contract
{
    public class MomentCheckDetailRequest
    {
        /// <summary>
        /// 动态Id
        /// </summary>
        public Guid MomentId { get; set; }
    }

    public class MomentCheckDetailResponse
    {
        public List<MomentCheckDetailType> MomentList { get; set; }
    }

    public class MomentCheckDetailType : MomentDetailType
    {
        /// <summary>
        /// 申请人数描述
        /// </summary>
        public string ApplyCountDesc { get; set; }

        public MomentStateEnum State { get; set; }

        /// <summary>
        /// 状态描述
        /// </summary>
        public string StateDesc { get; set; }

        /// <summary>
        /// 状态文本采用字体颜色
        /// </summary>
        public string TextColor { get; set; }


        public List<ApplyItem> ApplyList { get; set; }
    }
}
