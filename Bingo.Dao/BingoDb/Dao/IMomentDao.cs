using Bingo.Dao.BingoDb.Entity;
using System;
using System.Collections.Generic;

namespace Bingo.Dao.BingoDb.Dao
{
    public interface IMomentDao
    {
        MomentEntity GetMomentByMomentId(Guid momentId);

        List<MomentEntity> GetMomentListByParam();

        List<MomentEntity> GetMomentListByUid(long uid);

        int Insert(MomentEntity entity);

    }
}
