using IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.Model;
using System.Data.SQLite;
using DBUtility;
using System.Data;

namespace SQLiteDAL
{
    public class DBLibrary : IDBLibrary
    {
        private readonly string sqlUpdateFormat = @"UPDATE " + DataAccess.TABLE_NAME_LIBRARY + " SET PATH = @PATH, LEVEL = @LEVEL, ISDELETED = @ISDELETED WHERE ID = {0};";
        private readonly string sqlInsertFormat = "INSERT INTO {0} (ID, PATH, LEVEL, ISDELETED) VALUES (NULL, @PATH, @LEVEL, @ISDELETED);" + DataAccess.SQL_SELECT_ID_LAST;
        public void DeleteItems(IList<LibraryItemEntity> items)
        {
            throw new NotImplementedException("Do not delete anything!");
        }

        public IList<LibraryItemEntity> GetItems()
        {
            IList<LibraryItemEntity> items = new List<LibraryItemEntity>();
            string sqlSelect = "SELECT * FROM " + DataAccess.TABLE_NAME_LIBRARY;
            SQLiteDataReader dr = SqliteHelper.ExecuteReader(DataAccess.ConnectionStringProfile, CommandType.Text, sqlSelect, null);
            while (dr.Read())
            {
                if (dr.IsDBNull(1))
                    continue;
                LibraryItemEntity item = new LibraryItemEntity();
                item.ID = dr.GetInt32(0);
                item.Path = dr.GetString(1);
                item.Level = dr.GetInt32(2);
                item.IsDeleted = dr.GetInt32(3) == 1 ? true : false;
                items.Add(item);
            }
            dr.Close();
            return items;
        }

        public int InsertItems(IList<LibraryItemEntity> items)
        {
            int iSuccessRows = 0;
            if (0 == items.Count)
                return iSuccessRows;

            string sqlInsert = string.Format(sqlInsertFormat, DataAccess.TABLE_NAME_LIBRARY);
            SQLiteConnection conn = new SQLiteConnection(DataAccess.ConnectionStringProfile);
            conn.Open();
            SQLiteTransaction trans = conn.BeginTransaction(IsolationLevel.ReadCommitted);
            SQLiteParameter[] parms = {
                new SQLiteParameter("@PATH", DbType.String),
                new SQLiteParameter("@LEVEL", DbType.Int32),
                new SQLiteParameter("@ISDELETED", DbType.Int32)
                    };

            try
            {
                //SqliteHelper.ExecuteNonQuery(trans, CommandType.Text, sqlDelete, parms);

                foreach (LibraryItemEntity item in items)
                {
                    parms[0].Value = item.Path;
                    parms[1].Value = item.Level;
                    parms[2].Value = item.IsDeleted;
                    //iSuccessRows += SqliteHelper.ExecuteNonQuery(trans, CommandType.Text, sqlInsert, parms);
                    object objRel = SqliteHelper.ExecuteScalar(DataAccess.ConnectionStringProfile, CommandType.Text, sqlInsert, parms);
                    if (null != objRel)
                    {
                        iSuccessRows++;
                        int id = Convert.ToInt32(objRel);
                        item.ID = id;
                    }
                }

                trans.Commit();
            }
            catch (Exception e)
            {
                trans.Rollback();
                throw new ApplicationException(e.Message);
            }
            finally
            {
                conn.Close();
            }

            return iSuccessRows;
        }

        public int UpdateItems(IList<LibraryItemEntity> items)
        {
            int iSuccessRows = 0;
            SQLiteConnection conn = new SQLiteConnection(DataAccess.ConnectionStringProfile);
            conn.Open();
            SQLiteTransaction trans = conn.BeginTransaction(IsolationLevel.ReadCommitted);
            SQLiteParameter[] parms = {
                new SQLiteParameter("@PATH", DbType.String),
                new SQLiteParameter("@LEVEL", DbType.Int32),
                new SQLiteParameter("@ISDELETED", DbType.Int32)
                    };

            try
            {
                foreach (LibraryItemEntity file in items)
                {
                    string sqlUpdate = string.Format(sqlUpdateFormat, file.ID);
                    parms[0].Value = file.Path;
                    parms[1].Value = file.Level;
                    parms[2].Value = file.IsDeleted;
                    iSuccessRows += SqliteHelper.ExecuteNonQuery(trans, CommandType.Text, sqlUpdate, parms);
                }

                trans.Commit();
            }
            catch (Exception e)
            {
                trans.Rollback();
                throw new ApplicationException(e.Message);
            }
            finally
            {
                conn.Close();
            }
            return iSuccessRows;
        }
    }
}
