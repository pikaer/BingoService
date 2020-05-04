using Bingo.Biz.Interface;
using Bingo.Dao.BingoDb.Dao;
using Bingo.Dao.BingoDb.Dao.Impl;
using Bingo.Dao.BingoDb.Entity;
using Bingo.Model.Base;
using Bingo.Model.Common;
using Bingo.Utils;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bingo.Biz.Impl.Builder
{
    public static class ApplyBuilder
    {
        private readonly static IUserInfoBiz uerInfoBiz = SingletonProvider<UserInfoBiz>.Instance;
        private readonly static IApplyInfoDao applyInfoDao = SingletonProvider<ApplyInfoDao>.Instance;
        private readonly static IApplyDetailDao applyDetailDao = SingletonProvider<ApplyDetailDao>.Instance;

        /// <summary>
        /// 获取动态申请列表
        /// </summary>
        /// <param name="momentId">动态Id</param>
        /// <param name="passApply">仅仅查看申请通过数据</param>
        /// <returns></returns>
        public static List<ApplyItem> GetApplyList(Guid momentId, bool isValid, RequestHead head,long momentUId = 0)
        {
            var applyList = applyInfoDao.GetListByMomentId(momentId);
            if (applyList.IsNullOrEmpty())
            {
                return null;
            }
            if (isValid)
            {
                applyList = applyList.Where(a => a.ApplyState == ApplyStateEnum.申请通过).ToList();
            }
            var resultList = new List<ApplyItem>();
            for (var index = 0; index < applyList.Count; index++)
            {
                var apply = applyList[index];
                var userInfo = uerInfoBiz.GetUserInfoByUid(apply.UId);
                if (userInfo == null)
                {
                    continue;
                }
                var result = new ApplyItem()
                {
                    ApplyId = apply.ApplyId,
                    StateDesc = StateDescMap(apply.ApplyState),
                    TextColor = TextColorMap(apply.ApplyState),
                    UserInfo = UserInfoBuilder.BuildUserInfo(userInfo, head),
                    ShowBorder = index != applyList.Count - 1,
                    CreateTimeDesc = DateTimeHelper.GetDateDesc(apply.CreateTime, true),
                };
                if (!isValid)
                {
                    var applyDetaiList = applyDetailDao.GetListByApplyId(apply.ApplyId);
                    if (applyDetaiList.NotEmpty())
                    {
                        applyDetaiList = applyDetaiList.Where(a => a.UId != momentUId).OrderByDescending(a => a.CreateTime).ToList();
                        if (applyDetaiList.NotEmpty())
                        {
                            result.Content = applyDetaiList[0].Content;
                        }
                    }
                }

                resultList.Add(result);
            }
            return resultList;
        }


        public static List<ApplyDetailItem> GetApplyDetails(Guid applyId, RequestHead head)
        {
            var applyDetaiList = applyDetailDao.GetListByApplyId(applyId);
            if (applyDetaiList.IsNullOrEmpty())
            {
                return null;
            }
            var resultList = new List<ApplyDetailItem>();
            var resultDic = GetUserInfo(applyDetaiList, head);
            foreach (var item in applyDetaiList)
            {
                resultList.Add(new ApplyDetailItem()
                {
                    CreateTimeDesc = DateTimeHelper.GetDateDesc(item.CreateTime, true),
                    Content = item.Content,
                    UserInfo = resultDic[item.UId]
                });
            }
            return resultList;
        }

        public static string GetApplyCountDesc(List<ApplyInfoEntity> applyList)
        {
            if (applyList.IsNullOrEmpty())
            {
                return "已通过:0人 待处理:0人";
            }

            var text = new StringBuilder();
            var passCount = applyList.Count(a => a.ApplyState == ApplyStateEnum.申请通过);
            text.AppendFormat("已通过:{0}人 ", passCount);

            var askCount = applyList.Count(a => a.ApplyState == ApplyStateEnum.申请中);
            text.AppendFormat("待处理:{0}人", askCount);

            return text.ToString();
        }

        public static string GetApplyCountColor(List<ApplyInfoEntity> applyList)
        {
            if (applyList.IsNullOrEmpty())
            {
                //黑色
                return "black";
            }
            var askCount = applyList.Count(a => a.ApplyState == ApplyStateEnum.申请中);
            if (askCount > 0)
            {
                //红色
                return CommonConst.Color_Red;
            }

            //黑色
            return "black";
        }


        private static Dictionary<long, UserInfoType> GetUserInfo(List<ApplyDetailEntity> applyDetaiList, RequestHead head)
        {
            var resultDic = new Dictionary<long, UserInfoType>();
            foreach (var item in applyDetaiList.GroupBy(a => a.UId))
            {
                resultDic.Add(item.Key, UserInfoBuilder.BuildUserInfo(uerInfoBiz.GetUserInfoByUid(item.Key), head));
            }
            return resultDic;
        }

        public static bool IsOverCount(MomentEntity moment)
        {
            return moment.ApplyCount >= moment.NeedCount;
        }

        public static string TextColorMap(ApplyStateEnum applyState)
        {
            switch (applyState)
            {
                case ApplyStateEnum.申请中:
                    //红色
                    return CommonConst.Color_Red;
                case ApplyStateEnum.申请通过:
                    //绿色
                    return CommonConst.Color_Green;
                case ApplyStateEnum.被拒绝:
                case ApplyStateEnum.申请已撤销:
                case ApplyStateEnum.永久拉黑:
                default:
                    //黑色
                    return CommonConst.Color_Black;
            }
        }

        public static string BtnTextMap(ApplyStateEnum applyState)
        {
            switch (applyState)
            {
                case ApplyStateEnum.申请中:
                    return "撤销申请";
                case ApplyStateEnum.被拒绝:
                case ApplyStateEnum.申请已撤销:
                    return "再次申请";
                case ApplyStateEnum.申请通过:
                case ApplyStateEnum.永久拉黑:
                default:
                    return "";
            }
        }

        public static string StateDescMap(ApplyStateEnum applyState)
        {
            switch (applyState)
            {
                case ApplyStateEnum.申请中:
                    return "待通过";
                case ApplyStateEnum.被拒绝:
                    return "已拒绝";
                case ApplyStateEnum.申请已撤销:
                    return "对方撤销申请";
                case ApplyStateEnum.申请通过:
                    return "已通过";
                case ApplyStateEnum.永久拉黑:
                    return "已拉黑对方";
                default:
                    return "";
            }
        }


        public static string BtnActionMap(ApplyStateEnum applyState)
        {
            switch (applyState)
            {
                case ApplyStateEnum.申请中:
                    return "cancel";
                case ApplyStateEnum.被拒绝:
                case ApplyStateEnum.申请已撤销:
                    return "reask";
                case ApplyStateEnum.申请通过:
                case ApplyStateEnum.永久拉黑:
                default:
                    return "";
            }
        }
    }
}
