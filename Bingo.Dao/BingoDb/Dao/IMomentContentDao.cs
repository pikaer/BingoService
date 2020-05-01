using Bingo.Dao.BingoDb.Entity;
using System;
using System.Collections.Generic;

namespace Bingo.Dao.BingoDb.Dao
{
    public interface IMomentContentDao
    {
        List<MomentContentEntity> GetContentListByMomentId(Guid momentId);

        int Insert(MomentContentEntity entity);
    }
}
