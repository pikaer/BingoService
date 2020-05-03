using Bingo.Dao.BingoDb.Entity;
using System;
using System.Collections.Generic;

namespace Bingo.Dao.BingoDb.Dao
{
    public interface IApplyInfoDao
    {
        bool Insert(ApplyInfoEntity entity);

        bool UpdateState(ApplyStateEnum applyState, Guid applyId);

        ApplyInfoEntity GetByMomentIdAndUId(Guid momentId, long uId);

        ApplyInfoEntity GetByApplyId(Guid applyId);

        List<ApplyInfoEntity> GetListByMomentId(Guid momentId);

        List<ApplyInfoEntity> GetListByMomentUId(long uId);

        List<ApplyInfoEntity> GetListByUId(long uId);
    }
}
