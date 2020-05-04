namespace Bingo.Model.Common
{
    public class ApplyDetailItem
    {
        /// <summary>
        /// 用户信息
        /// </summary>
        public UserInfoType UserInfo { get; set; }

        /// <summary>
        /// 提交时间
        /// </summary>
        public string CreateTimeDesc { get; set; }
        /// <summary>
        /// 申请内容
        /// </summary>
        public string Content { get; set; }
    }
}
