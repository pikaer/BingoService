namespace Bingo.Model.DTO
{
    public class WeChatMsgSecCheckRequestDTO
    {
        /// <summary>
        /// 要检测的文本内容，长度不超过 500KB
        /// </summary>
        public string content { get; set; }
    }
}
