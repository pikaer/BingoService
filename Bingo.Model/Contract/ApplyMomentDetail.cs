using Bingo.Dao.BingoDb.Entity;
using Bingo.Model.Common;
using System;
using System.Collections.Generic;

namespace Bingo.Model.Contract
{
    public class ApplyMomentDetailRequest
    {
        /// <summary>
        /// 申请Id
        /// </summary>
        public Guid ApplyId { get; set; }
    }

    public class ApplyMomentDetailResponse : MomentDetailType
    {
        public bool BtnVisable { get; set; }

        /// <summary>
        /// 按钮文案
        /// </summary>
        public string BtnText { get; set; }

        /// <summary>
        /// 申请状态
        /// </summary>
        public ApplyStateEnum ApplyState { get; set; }

        /// <summary>
        /// 状态描述
        /// </summary>
        public string ApplyStateDesc { get; set; }

        /// <summary>
        /// 状态文本采用字体颜色
        /// </summary>
        public string TextColor { get; set; }

        /// <summary>
        /// reask=重新申请,cancel=取消申请
        /// </summary>
        public string NextAction { get; set; }

        public List<ApplyDetailItem> ApplyList { get; set; }
    }
}
