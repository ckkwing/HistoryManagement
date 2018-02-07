using HistoryManagement.Infrastructure.UIModel;
using IDAL.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Utilities;
using Utilities.Extension;

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
        public IEnumerable<LibraryItemEntity> LibraryItems
        {
            get
            {
                return libraryItems;
            }
        }

        private ObservableCollection<HistoryEntity> histories = new ObservableCollection<HistoryEntity>();
        public ObservableCollection<HistoryEntity> Histories
        {
            get
            {
                return histories;
            }
        }

        private IList<CategoryEntity> categories = new List<CategoryEntity>();
        public IEnumerable<CategoryEntity> Categories
        {
            get
            {
                return categories;
            }
        }
        #endregion

        public void Init()
        {
            DBHelper.InitDBProfile();
            RefreshDBData();
        }

        public void Uninit()
        {

        }

        public void RefreshDBData()
        {
            libraryItems.Clear();
            foreach(LibraryItemEntity libraryItem in DBHelper.GetLibraryItems())
            {
                libraryItems.Add(libraryItem);
            }

            histories.Clear();
            foreach (HistoryEntity history in DBHelper.GetHistories())
            {
                histories.Add(history);
            }

            categories.Clear();
            foreach (CategoryEntity category in DBHelper.GetCategories())
            {
                categories.Add(category);
            }
        }

        public int AddLibraryItems(IList<LibraryItemEntity> items)
        {
            if (items.IsNull())
                return 0;

            return DBHelper.AddLibraryItems(items);
        }

        public int AddLibraryItems(IList<UILibraryItemEntity> items)
        {
            if (items.IsNull())
                return 0;

            IList<LibraryItemEntity> dbItems = new List<LibraryItemEntity>();
            foreach (UILibraryItemEntity item in items)
            {
                LibraryItemEntity dbItem = UILibraryItemEntity.ToDBEntity(item);
                dbItems.Add(dbItem);
            }
            return DBHelper.AddLibraryItems(dbItems);
        }

        public int UpdateLibraryItems(IList<UILibraryItemEntity> items)
        {
            if (items.IsNull())
                return 0;

            IList<LibraryItemEntity> dbItems = new List<LibraryItemEntity>();
            foreach (UILibraryItemEntity item in items)
            {
                LibraryItemEntity dbItem = UILibraryItemEntity.ToDBEntity(item);
                dbItems.Add(dbItem);
            }
            return DBHelper.UpdateLibraryItems(dbItems);
        }

        public void UpdateLibraryItems(IList<LibraryItemEntity> list)
        {

        }

        #region Scanner

        public void StartScanAndUpdateDB(IList<UILibraryItemEntity> list)
        {
            //Action action = () => {
            //    HistoryScanner scanner = new HistoryScanner(CacheLibraryList);
            //    scanner.StartScan();
            //};

            //action.BeginInvoke(ar => {
            //    ThreadHelper.EndInvokeAction(ar);
            //}, action);
            var addList = list.Where(item => !LibraryItems.Any(i => 0 == string.Compare(i.Path, item.Path, true)));
            var updateList = list.Where(item => null != LibraryItems.FirstOrDefault(i => ((0 == string.Compare(i.Path, item.Path, true)) && i.Level != item.Level)));
            if (0 == addList.Count() && 0 == updateList.Count())
                return;

            Func<IEnumerable<DirectoryInfo>> function = () =>
            {
                AddLibraryItems(addList.ToList());
                UpdateLibraryItems(updateList.ToList());

                HistoryScanner scanner = new HistoryScanner(list);
                scanner.StartScan();

                var dirList = scanner.Results.Where(item => !Histories.Any(i => 0 == string.Compare(i.Path, item.FullName, true)));
                IList<HistoryEntity> historyList = new List<HistoryEntity>();
                foreach (var dir in dirList)
                {
                    HistoryEntity history = new HistoryEntity() { IsDeleted = false, Name = dir.Name, Path = dir.FullName, Comment = string.Empty };
                    historyList.Add(history);
                }
                DBHelper.AddHistoryItems(historyList);

                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                {
                    RefreshDBData();
                }));
                
                return scanner.Results;
            };

            function.BeginInvoke(ar => {
                IEnumerable<DirectoryInfo> results = ThreadHelper.EndInvokeFunc<IEnumerable<DirectoryInfo>>(ar);
            }, function);
        }

        #endregion

    }
}
