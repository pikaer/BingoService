using Infrastructure;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Bingo.Dao
{
    public abstract class DbBase
    {
        protected IDbConnection GetDbConnection()
        {
            var dbEnum = GetDbEnum();
            var dbName = Enum.GetName(dbEnum.GetType(), dbEnum);
            var connString = JsonSettingHelper.AppSettings[dbName];
            return new SqlConnection(connString);
        }

        /// <summary>
        /// 数据库连接字符串的名称
        /// </summary>
        protected enum DbEnum
        {
            FrameworkDb,
            BingoDb,
            LogDb
        }

        /// <summary>
        /// 设定数据库
        /// </summary>
        /// <returns></returns>
        protected abstract DbEnum GetDbEnum();
    }
}
