using IDAL;
using IDAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HistoryManagement.Infrastructure
{
    public sealed class DBHelper
    {
        private static readonly IDBEntity iDBEntity = DataAccess.CreateDBEntity();
        private static readonly IDBLibrary iDBLibrary = DataAccess.CreateLibrary();
        private static readonly IDBHistory iDBHistory = DataAccess.CreateHistory();
        private static readonly IDBCategory iDBCategory = DataAccess.CreateCategory();

        public static bool InitDBProfile()
        {
            return iDBEntity.InitDBProfile();
        }

        public static IList<LibraryItemEntity> GetLibraryItems()
        {
            return iDBLibrary.GetItems();
        }

        public static IList<HistoryEntity> GetHistories()
        {
            return iDBHistory.GetItems();
        }

        public static IList<CategoryEntity> GetCategories()
        {
            return iDBCategory.GetItems();
        }
    }
}
