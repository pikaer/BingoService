﻿using Bingo.Model.Common;
using System;
using System.Collections.Generic;

namespace Bingo.Model.Contract
{
    public class MomentDetailRequest
    {
        /// <summary>
        /// 动态Id
        /// </summary>
        public Guid MomentId { get; set; }
    }

    public class MomentDetailResponse : MomentDetailType
    {
        /// <summary>
        /// 按钮文案
        /// </summary>
        public string BtnText { get; set; }

        public bool BtnVisable { get; set; }

        /// <summary>
        /// 状态文本采用字体颜色
        /// </summary>
        public TextColorEnum TextColor { get; set; }

        /// <summary>
        /// 分享标志
        /// </summary>
        public bool ShareFlag { get; set; }

        /// <summary>
        /// 是否已申请（true,是展示跳转详情）
        /// </summary>
        public bool ApplyFlag { get; set; }

        /// <summary>
        /// 是否可以申请
        /// </summary>
        public bool AskFlag { get; set; }

        public List<ApplyDetailItem> ApplyList { get; set; }
    }
}