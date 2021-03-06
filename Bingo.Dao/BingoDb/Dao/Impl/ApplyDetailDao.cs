﻿using Bingo.Dao.BingoDb.Entity;
using Dapper;
using System;
using System.Collections.Generic;

namespace Bingo.Dao.BingoDb.Dao.Impl
{
    public class ApplyDetailDao : DbBase, IApplyDetailDao
    {
        protected override DbEnum GetDbEnum()
        {
            return DbEnum.BingoDb;
        }

        public List<ApplyDetailEntity> GetListByApplyId(Guid applyId)
        {
            var sql = @"SELECT * FROM dbo.ApplyDetail  Where ApplyId=@ApplyId order by CreateTime ";
            using var Db = GetDbConnection();
            return Db.Query<ApplyDetailEntity>(sql, new { ApplyId = applyId }).AsList();
        }

        public List<ApplyDetailEntity> GetListByMomentId(Guid momentId)
        {
            var sql = @"SELECT * FROM dbo.ApplyDetail Where MomentId=@MomentId and Type=1 order by CreateTime desc ";
            using var Db = GetDbConnection();
            return Db.Query<ApplyDetailEntity>(sql, new { MomentId = momentId }).AsList();
        }

        public bool Insert(ApplyDetailEntity entity)
        {
            var sql = @"INSERT INTO dbo.ApplyDetail
                                  (ApplyDetailId
                                  ,ApplyId
                                  ,UId
                                  ,Content
                                  ,MomentId
                                  ,Type
                                  ,CreateTime
                                  ,UpdateTime)
                            VALUES
                                  (@ApplyDetailId
                                  ,@ApplyId
                                  ,@UId
                                  ,@Content
                                  ,@MomentId
                                  ,@Type
                                  ,@CreateTime
                                  ,@UpdateTime)";
            using var Db = GetDbConnection();
            return Db.Execute(sql, entity) > 0;
        }
    }
}
