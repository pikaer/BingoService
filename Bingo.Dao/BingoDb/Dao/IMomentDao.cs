using Bingo.Dao.BingoDb.Entity;
using System;
using System.Collections.Generic;

namespace Bingo.Dao.BingoDb.Dao
{
    public interface IMomentDao
    {
        MomentEntity GetMomentByMomentId(Guid momentId);

        List<MomentEntity> GetMomentListByParam(bool offLine,int pageIndex, GenderEnum gender, SchoolStateEnum schoolState,List<string>ageList,double latitude,double longitude);

        List<MomentEntity> GetMomentListByUid(long uid);

        List<MomentEntity> GetMomentListByState(MomentStateEnum state);

        int Insert(MomentEntity entity);

        bool UpdateStopTime(Guid momentId);

        bool UpdateApplyCount(Guid momentId);

        bool UpdateMoment(MomentEntity entity);

        bool UpdateState(Guid momentId,MomentStateEnum momentState);

        bool Delete(Guid momentId);

        int PendingCount();
    }
}
