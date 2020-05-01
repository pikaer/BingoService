namespace Bingo.Model.DTO
{
    public class WeChatResponseDTO
    {
        /// <summary>
        /// 错误码 0:内容正常	87014:内容含有违法违规内容	
        /// </summary>
        public int Errcode { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrMsg { get; set; }
    }
}
