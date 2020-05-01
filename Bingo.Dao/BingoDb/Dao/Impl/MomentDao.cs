using Bingo.Dao.BingoDb.Entity;
using System;
using System.Collections.Generic;

namespace Bingo.Dao.BingoDb.Dao.Impl
{
    public class MomentDao : DbBase, IMomentDao
    {
        protected override DbEnum GetDbEnum()
        {
            return DbEnum.BingoDb;
        }

        public MomentEntity GetMomentByMomentId(Guid momentId)
        {
            throw new NotImplementedException();
        }

        public List<MomentEntity> GetMomentListByUid(long uid)
        {
            throw new NotImplementedException();
        }

        public int Insert(MomentEntity entity)
        {
            throw new NotImplementedException();
        }

        public List<MomentEntity> GetMomentListByParam()
        {
            throw new NotImplementedException();
        }
    }
}
