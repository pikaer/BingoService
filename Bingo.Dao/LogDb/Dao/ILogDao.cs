using Bingo.Dao.LogDb.Entity;
using System.Collections.Generic;

namespace Bingo.Dao.LogDb.Dao
{
    public interface ILogDao
    {
        int InsertLog(LogEntity logEntity);

        List<LogEntity> GetLogInfoList();
    }
}
