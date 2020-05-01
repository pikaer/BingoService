using Bingo.Dao.BingoDb.Entity;
using Dapper;
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

        public int Insert(MomentContentEntity entity)
        {
            var sql = @"INSERT INTO dbo.MomentContent
                                  (MomentContentId
                                  ,MomentId
                                  ,Title
                                  ,Content
                                  ,TagType
                                  ,CreateTime
                                  ,UpdateTime)
                            VALUES
                                  (@MomentContentId
                                  ,@MomentId
                                  ,@Title
                                  ,@Content
                                  ,@TagType
                                  ,@CreateTime
                                  ,@UpdateTime)";
            using var Db = GetDbConnection();
            return Db.Execute(sql, entity);
        }
    }
}
