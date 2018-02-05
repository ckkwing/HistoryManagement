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
            throw new NotImplementedException();
        }
    }
}
