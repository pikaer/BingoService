using Bingo.Dao.BingoDb.Entity;

namespace Bingo.Model.Contract
{
    public class UpdateUserInfoType
    {
        /// <summary>
        /// 头像路径
        /// </summary>
        public string Portrait { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 用户的性别，值为1时是男性，值为2时是女性，值为0时是未知
        /// </summary>
        public GenderEnum Gender { get; set; }

        /// <summary>
        /// 年级
        /// </summary>
        public GradeEnum Grade { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public SchoolStateEnum LiveState { get; set; }

        /// <summary>
        /// 学校名称
        /// </summary>
        public string SchoolName { get; set; }

        /// <summary>
        /// 生日（2018-08-20）
        /// </summary>
        public string BirthDate { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 微信号
        /// </summary>
        public string WeChatNo { get; set; }

        /// <summary>
        /// QQ号
        /// </summary>
        public string QQNo { get; set; }
    }
}
