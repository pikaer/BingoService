using Bingo.Dao.BingoDb.Entity;
using System;
using System.Collections.Generic;

namespace Bingo.Dao.BingoDb.Dao.Impl
{
    public class MomentContentDao : DbBase, IMomentContentDao
    {
        protected override DbEnum GetDbEnum()
        {
            return DbEnum.BingoDb;
        }
        public List<MomentContentEntity> GetContentListByMomentId(Guid momentId)
        {
            throw new NotImplementedException();
        }

        
    }
}
