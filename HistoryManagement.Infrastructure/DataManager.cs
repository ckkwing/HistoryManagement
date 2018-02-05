using IDAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HistoryManagement.Infrastructure
{
    public sealed class DataManager
    {
        public static DataManager Instance
        {
            get { return Nested.instance; }
        }

        class Nested
        {
            static Nested() { }
            internal static readonly DataManager instance = new DataManager();
        }

        DataManager() { }

        #region Property
        private IList<LibraryItemEntity> libraryItems = new List<LibraryItemEntity>();
        public IList<LibraryItemEntity> LibraryItems
        {
            get
            {
                return libraryItems;
            }

            set
            {
                libraryItems = value;
            }
        }

        private IList<HistoryEntity> histories = new List<HistoryEntity>();
        public IList<HistoryEntity> Histories
        {
            get
            {
                return histories;
            }

            set
            {
                histories = value;
            }
        }

        private IList<CategoryEntity> categories = new List<CategoryEntity>();
        public IList<CategoryEntity> Categories
        {
            get
            {
                return categories;
            }

            set
            {
                categories = value;
            }
        }
        #endregion

        public void Init()
        {
            RefreshDBData();
        }

        public void Uninit()
        {

        }

        public void RefreshDBData()
        {
            LibraryItems.Clear();
            foreach(LibraryItemEntity libraryItem in DBHelper.GetLibraryItems())
            {
                LibraryItems.Add(libraryItem);
            }

            Histories.Clear();
            foreach (HistoryEntity history in DBHelper.GetHistories())
            {
                Histories.Add(history);
            }

            Categories.Clear();
            foreach (CategoryEntity category in DBHelper.GetCategories())
            {
                Categories.Add(category);
            }
        }
        
    }
}
