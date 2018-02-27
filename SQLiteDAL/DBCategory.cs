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
    public class DBCategory : IDBCategory
    {
        private readonly string sqlInsertFormat = "INSERT INTO {0} (ID, NAME, DESCRIPTION) VALUES (NULL, @NAME, @DESCRIPTION);" + DataAccess.SQL_SELECT_ID_LAST;

        public void DeleteItems(IList<CategoryEntity> items)
        {
            throw new NotImplementedException("Do not delete anything!");
        }

        public IList<CategoryEntity> GetItems()
        {
            IList<CategoryEntity> items = new List<CategoryEntity>();
            string sqlSelect = "SELECT * FROM " + DataAccess.TABLE_NAME_CATEGORY;
            SQLiteDataReader dr = SqliteHelper.ExecuteReader(DataAccess.ConnectionStringProfile, CommandType.Text, sqlSelect, null);
            while (dr.Read())
            {
                if (dr.IsDBNull(1))
                    continue;
                CategoryEntity item = new CategoryEntity();
                item.ID = dr.GetInt32(0);
                item.Name = dr.GetString(1);
                item.Description = dr.GetString(2);
                items.Add(item);
            }
            dr.Close();
            return items;
        }

        public int InsertItems(IList<CategoryEntity> items)
        {
            int iSuccessRows = 0;
            if (0 == items.Count)
                return iSuccessRows;

            string sqlInsert = string.Format(sqlInsertFormat, DataAccess.TABLE_NAME_CATEGORY);
            SQLiteConnection conn = new SQLiteConnection(DataAccess.ConnectionStringProfile);
            conn.Open();
            SQLiteTransaction trans = conn.BeginTransaction(IsolationLevel.ReadCommitted);
            SQLiteParameter[] parms = {
                new SQLiteParameter("@NAME", DbType.String),
                new SQLiteParameter("@DESCRIPTION", DbType.String)
            };

            try
            {
                //SqliteHelper.ExecuteNonQuery(trans, CommandType.Text, sqlDelete, parms);

                foreach (CategoryEntity item in items)
                {
                    parms[0].Value = item.Name;
                    parms[1].Value = item.Description;
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
