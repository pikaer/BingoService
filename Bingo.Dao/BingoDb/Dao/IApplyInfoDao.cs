using Bingo.Dao.BingoDb.Entity;
using System;
using System.Collections.Generic;

namespace Bingo.Dao.BingoDb.Dao
{
    public interface IApplyInfoDao
    {
        bool Insert(ApplyInfoEntity entity);

        List<ApplyInfoEntity> GetListByMomentId(Guid momentId);
    }
}
