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
            throw new NotImplementedException();
        }
    }
}
