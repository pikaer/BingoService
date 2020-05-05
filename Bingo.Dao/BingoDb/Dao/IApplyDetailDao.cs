using Bingo.Dao.BingoDb.Entity;
using System;
using System.Collections.Generic;

namespace Bingo.Dao.BingoDb.Dao
{
    public interface IApplyDetailDao
    {
        bool Insert(ApplyDetailEntity entity);

        List<ApplyDetailEntity> GetListByApplyId(Guid applyId);

        List<ApplyDetailEntity> GetListByMomentId(Guid momentId);
    }
}
