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
                var result = new ApplyDetailItem()
                {
                    CreateTimeDesc = DateTimeHelper.GetDateDesc(item.CreateTime, true),
                    Content = item.Content,
                    UserInfo = resultDic[item.UId]
                };
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

        public static TextColorEnum TextColorMap(ApplyStateEnum applyState)
        {
            switch (applyState)
            {
                case ApplyStateEnum.申请中:
                    return TextColorEnum.Red;
                case ApplyStateEnum.申请通过:
                    return TextColorEnum.Green;
                case ApplyStateEnum.被拒绝:
                case ApplyStateEnum.申请已撤销:
                case ApplyStateEnum.永久拉黑:
                default:
                    return TextColorEnum.Default;
            }
        }
    }
}
