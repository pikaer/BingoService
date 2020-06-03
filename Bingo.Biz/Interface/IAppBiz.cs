using Bingo.Dao.BingoDb.Entity;
using System.Collections.Generic;

namespace Bingo.Biz.Interface
{
    public interface IAppBiz
    {
        string GetOpenId(string loginCode);

        /// <summary>
        /// 文本安全校验
        /// </summary>
        bool MsgSecCheck(string message);

        /// <summary>
        /// 活动报名通知
        /// </summary>
        /// <param name="moment">动态详情</param>
        /// <param name="targetUserId">目标用户</param>
        /// <param name="joinSuccess">是否参与成功</param>
        /// <param name="joinMsg">消息提示</param>
        bool Send_Activity_Join_Msg(MomentEntity moment, long targetUserId, bool joinSuccess, string joinMsg);

        /// <summary>
        /// 活动取消通知
        /// </summary>
        /// <param name="moment">动态详情</param>
        /// <param name="userIds">目标用户</param>
        bool Send_Activity_Cancel_Msg(MomentEntity moment, List<long> userIds);

        /// <summary>
        /// 新动态发布提醒
        /// </summary>
        /// <param name="moment">动态详情</param>
        /// <param name="publishSuccess">发布审核是否通过</param>
        /// <param name="msg">审核信息</param>
        bool Send_Moment_Publish_Msg(MomentEntity moment, bool publishSuccess, string msg);

        /// <summary>
        /// 报名审核通知
        /// </summary>
        /// <param name="moment">动态详情</param>
        /// <param name="targetUserId">目标用户</param>
        bool Send_Moment_Join_Msg(MomentEntity moment, long targetUserId);
    }
}
