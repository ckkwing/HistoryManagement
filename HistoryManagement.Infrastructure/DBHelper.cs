using IDAL;
using IDAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HistoryManagement.Infrastructure
{
    internal sealed class DBHelper
    {
        private static readonly IDBEntity iDBEntity = DataAccess.CreateDBEntity();
        private static readonly IDBLibrary iDBLibrary = DataAccess.CreateLibrary();
        private static readonly IDBHistory iDBHistory = DataAccess.CreateHistory();
        private static readonly IDBCategory iDBCategory = DataAccess.CreateCategory();

        internal static bool InitDBProfile()
        {
            return iDBEntity.InitDBProfile();
        }

        internal static IList<LibraryItemEntity> GetLibraryItems()
        {
            return iDBLibrary.GetItems();
        }

        internal static int AddLibraryItems(IList<LibraryItemEntity> items)
        {
            return iDBLibrary.InsertItems(items);
        }

        internal static int UpdateLibraryItems(IList<LibraryItemEntity> items)
        {
            return iDBLibrary.UpdateItems(items);
        }

        internal static void DeleteLibraryItems(IList<LibraryItemEntity> items)
        {
            iDBLibrary.DeleteItems(items);
        }

        internal static IList<HistoryEntity> GetHistories()
        {
            return iDBHistory.GetItems();
        }

        internal static int AddHistoryItems(IList<HistoryEntity> items)
        {
            return iDBHistory.InsertItems(items);
        }

        internal static IList<CategoryEntity> GetCategories()
        {
            return iDBCategory.GetItems();
        }

        internal static int AddCategories(IList<CategoryEntity> items)
        {
            return iDBCategory.InsertItems(items);
        }
    }
}
