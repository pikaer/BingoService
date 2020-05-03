using Bingo.Dao.BingoDb.Dao;
using Bingo.Dao.BingoDb.Dao.Impl;
using Bingo.Dao.BingoDb.Entity;
using Bingo.Utils;
using Infrastructure;
using System;

namespace Bingo.Biz.Impl.Builder
{
    public static class MomentBuilder
    {
        private readonly static IMomentDao momentDao = SingletonProvider<MomentDao>.Instance;
        private readonly static RedisHelper redisClient = RedisHelper.Instance;
        public static MomentEntity GetMoment(Guid momentId)
        {
            string cacheKey = RedisKeyConst.MomentCacheKey(momentId);
            var moment = redisClient.Get<MomentEntity>(cacheKey);
            if (moment!=null)
            {
                return moment;
            }
            moment = momentDao.GetMomentByMomentId(momentId);
            if (moment != null)
            {
                //缓存一个月
                redisClient.Set(cacheKey, moment, RedisKeyConst.UserInfoCacheSecond);
            }
            return moment;
        }
    }
}
