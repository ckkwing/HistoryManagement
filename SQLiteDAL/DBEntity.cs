using DBUtility;
using IDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteDAL
{
    public class DBEntity : IDBEntity
    {
        public bool InitDBProfile()
        {
            if (!System.IO.File.Exists(DataAccess.DBFilePath))
            {
                SQLiteConnection.CreateFile(DataAccess.DBFilePath);
            }

            CreateLibraryTable();
            CreateCategoryTable();
            CreateHistoryTable();
            return true;
        }

        private void CreateLibraryTable()
        {
            if (Helper.TabbleIsExist(DataAccess.TABLE_NAME_LIBRARY))
                return;
            string sqlFormat = @"CREATE TABLE {0}(
                                                                    ID INTEGER PRIMARY KEY   AUTOINCREMENT, 
                                                                    PATH TEXT NOT NULL, 
                                                                    LEVEL INTEGER NOT NULL,
                                                                    ISDELETED INTEGER DEFAULT 0
                                                                    );";
            string sql = string.Format(sqlFormat, DataAccess.TABLE_NAME_LIBRARY);
            SqliteHelper.ExecuteNonQuery(DataAccess.ConnectionStringProfile, CommandType.Text, sql, null);
        }

        private void CreateCategoryTable()
        {
            if (Helper.TabbleIsExist(DataAccess.TABLE_NAME_CATEGORY))
                return;
            string sqlFormat = @"CREATE TABLE {0}(
                                                                    ID INTEGER PRIMARY KEY   AUTOINCREMENT, 
                                                                    NAME TEXT NOT NULL, 
                                                                    DESCRIPTION TEXT
                                                                    );";
            string sql = string.Format(sqlFormat, DataAccess.TABLE_NAME_CATEGORY);
            SqliteHelper.ExecuteNonQuery(DataAccess.ConnectionStringProfile, CommandType.Text, sql, null);
        }

        private void CreateHistoryTable()
        {
            if (Helper.TabbleIsExist(DataAccess.TABLE_NAME_HISTORY))
                return;
            string sqlFormat = @"CREATE TABLE {0}(
                                                                    ID INTEGER PRIMARY KEY   AUTOINCREMENT,
                                                                    PATH TEXT NOT NULL,
                                                                    NAME TEXT NOT NULL,
                                                                    STAR INTEGER,
                                                                    COMMENT TEXT,
                                                                    CATEGORYIDS TEXT,
                                                                    ISDELETED INTEGER DEFAULT 0
                                                                    );";
            string sql = string.Format(sqlFormat, DataAccess.TABLE_NAME_HISTORY);
            SqliteHelper.ExecuteNonQuery(DataAccess.ConnectionStringProfile, CommandType.Text, sql, null);
        }
    }
}
