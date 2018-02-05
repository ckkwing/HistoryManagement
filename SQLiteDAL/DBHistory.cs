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
                    item.CategoryIDs.Add(Convert.ToInt32(arrayCategory[i]));
                }
                item.IsDeleted = dr.GetInt32(6) == 1 ? true : false;
                items.Add(item);
            }
            dr.Close();
            return items;
        }

        public int InsertItems(IList<HistoryEntity> items)
        {
            throw new NotImplementedException();
        }
    }
}
