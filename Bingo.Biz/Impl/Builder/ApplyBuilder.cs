using Bingo.Biz.Interface;
using Bingo.Dao.BingoDb.Dao;
using Bingo.Dao.BingoDb.Dao.Impl;
using Bingo.Dao.BingoDb.Entity;
using Bingo.Model.Common;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bingo.Biz.Impl.Builder
{
    public static class ApplyBuilder
    {
        private readonly static IUserInfoBiz uerInfoBiz = SingletonProvider<UserInfoBiz>.Instance;
        private readonly static IApplyInfoDao applyInfoDao = SingletonProvider<ApplyInfoDao>.Instance;
        private readonly static IApplyDetailDao applyDetailDao = SingletonProvider<ApplyDetailDao>.Instance;

        public static List<ApplyDetailItem> GetApplyList(Guid momentId)
        {
            var applyList = applyInfoDao.GetListByMomentId(momentId);
            if (applyList.IsNullOrEmpty())
            {
                return null;
            }
            var resultList = new List<ApplyDetailItem>();
            foreach (var apply in applyList)
            {
                var userInfo = uerInfoBiz.GetUserInfoByUid(apply.UId);
                if (userInfo == null)
                {
                    continue;
                }
                var result = new ApplyDetailItem()
                {
                    UserInfo = UserInfoBuilder.BuildUserInfo(userInfo),
                    CreateTimeDesc = DateTimeHelper.GetDateDesc(apply.CreateTime, true),
                };
                resultList.Add(result);
            }
            return resultList;
        }


        public static List<ApplyDetailItem> GetApplyDetails(Guid applyId)
        {
            var applyDetaiList = applyDetailDao.GetListByApplyId(applyId);
            if (applyDetaiList.IsNullOrEmpty())
            {
                return null;
            }
            var resultList = new List<ApplyDetailItem>();
            var resultDic = GetUserInfo(applyDetaiList);
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

        private static Dictionary<long, UserInfoType> GetUserInfo(List<ApplyDetailEntity> applyDetaiList)
        {
            var resultDic = new Dictionary<long, UserInfoType>();
            foreach (var item in applyDetaiList.GroupBy(a => a.UId))
            {
                resultDic.Add(item.Key, UserInfoBuilder.BuildUserInfo(uerInfoBiz.GetUserInfoByUid(item.Key)));
            }
            return resultDic;
        }

        public static bool IsOverCount(MomentEntity moment)
        {
            var aplyList = applyInfoDao.GetListByMomentId(moment.MomentId);
            if (aplyList.IsNullOrEmpty())
            {
                return false;
            }
            var usableList = aplyList.Where(a => a.ApplyState == ApplyStateEnum.申请通过).ToList();
            if (usableList.NotEmpty() && usableList.Count >= moment.NeedCount)
            {
                return true;
            }
            return false;
        }

        public static string TextColorMap(ApplyStateEnum applyState)
        {
            switch (applyState)
            {
                case ApplyStateEnum.申请中:
                    //红色
                    return "#fa6e4f";
                case ApplyStateEnum.申请通过:
                    //绿色
                    return "#2cbb60";
                case ApplyStateEnum.被拒绝:
                case ApplyStateEnum.申请已撤销:
                case ApplyStateEnum.永久拉黑:
                default:
                    //黑色
                    return "#8e8e8e";
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
