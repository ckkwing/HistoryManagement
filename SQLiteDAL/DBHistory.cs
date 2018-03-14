using IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.Model;
using System.Data.SQLite;
using System.Data;
using DBUtility;

namespace SQLiteDAL
{
    public class DBHistory : IDBHistory
    {
        private readonly string sqlInsertFormat = "INSERT INTO {0} (ID, PATH, NAME, STAR, COMMENT, CATEGORYIDS, ISDELETED, LIBRARYID) VALUES (NULL, @PATH, @NAME, @STAR, @COMMENT, @CATEGORYIDS, @ISDELETED, @LIBRARYID);" + DataAccess.SQL_SELECT_ID_LAST;

        public void DeleteItems(IList<HistoryEntity> items)
        {
            throw new NotImplementedException("Do not delete anything!");
        }

        public IList<HistoryEntity> GetItems()
        {
            IList<HistoryEntity> items = new List<HistoryEntity>();
            string sqlSelect = "SELECT * FROM " + DataAccess.TABLE_NAME_HISTORY;
            SQLiteDataReader dr = SqliteHelper.ExecuteReader(DataAccess.ConnectionStringProfile, CommandType.Text, sqlSelect, null);
            while (dr.Read())
            {
                if (dr.IsDBNull(1))
                    continue;
                HistoryEntity item = new HistoryEntity();
                item.ID = dr.GetInt32(0);
                item.Path = dr.GetString(1);
                item.Name = dr.GetString(2);
                item.Star = dr.GetInt32(3);
                item.Comment = dr.GetString(4);
                string strCategories = dr.GetString(5);
                string[] arrayCategory = strCategories.Split(new char[] { ';' });
                for(int i =0; i<arrayCategory.Length;i++)
                {
                    if (string.IsNullOrEmpty(arrayCategory[i]))
                        continue;
                    item.CategoryIDs.Add(Convert.ToInt32(arrayCategory[i]));
                }
                item.IsDeleted = dr.GetInt32(6) == 1 ? true : false;
                item.LibraryID = dr.GetInt32(7);
                items.Add(item);
            }
            dr.Close();
            return items;
        }

        public int InsertItems(IList<HistoryEntity> items)
        {
            int iSuccessRows = 0;
            if (0 == items.Count)
                return iSuccessRows;

            string sqlInsert = string.Format(sqlInsertFormat, DataAccess.TABLE_NAME_HISTORY);
            SQLiteConnection conn = new SQLiteConnection(DataAccess.ConnectionStringProfile);
            conn.Open();
            SQLiteTransaction trans = conn.BeginTransaction(IsolationLevel.ReadCommitted);
            SQLiteParameter[] parms = {
                new SQLiteParameter("@PATH", DbType.String),
                new SQLiteParameter("@NAME", DbType.String),
                new SQLiteParameter("@STAR", DbType.Int32),
                new SQLiteParameter("@COMMENT", DbType.String),
                new SQLiteParameter("@CATEGORYIDS", DbType.String),
                new SQLiteParameter("@ISDELETED", DbType.Int32),
                new SQLiteParameter("@LIBRARYID", DbType.Int32)
                    };

            try
            {
                //SqliteHelper.ExecuteNonQuery(trans, CommandType.Text, sqlDelete, parms);

                foreach (HistoryEntity item in items)
                {
                    parms[0].Value = item.Path;
                    parms[1].Value = item.Name;
                    parms[2].Value = item.Star;
                    parms[3].Value = item.Comment;
                    string ids = string.Empty;
                    foreach(int id in item.CategoryIDs)
                    {
                        ids += id.ToString();
                    }
                    parms[4].Value = ids;
                    parms[5].Value = item.IsDeleted;
                    parms[6].Value = item.LibraryID;
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
    }
}
