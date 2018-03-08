using HistoryManagement.Infrastructure.Events;
using HistoryManagement.Infrastructure.UIModel;
using IDAL.Model;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
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

        DataManager()
        {
            
        }

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

            //set
            //{
            //    categories = value;
            //}
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

            try
            {
                IEventAggregator eventAggregator = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IEventAggregator>();
                if (!eventAggregator.IsNull())
                    eventAggregator.GetEvent<DBDataRefreshedEvents>().Publish(new DBDataRefreshedEventEventArgs() { DBDataType = DBDataType.All });
            }
            catch(Exception e)
            {
                NLogger.LogHelper.UILogger.Debug(e);
            }
        }

        public int AddLibraryItems(IList<LibraryItemEntity> items)
        {
            if (items.IsNull())
                return 0;

            return DBHelper.AddLibraryItems(items);
        }

        public int UpdateLibraryItems(IList<LibraryItemEntity> items)
        {
            if (items.IsNull())
                return 0;
            
            return DBHelper.UpdateLibraryItems(items);
        }

        #region Scanner

        public void StartScanAndUpdateDB(IList<LibraryItemEntity> addList, IList<LibraryItemEntity> updateList)
        {
            //Action action = () => {
            //    HistoryScanner scanner = new HistoryScanner(CacheLibraryList);
            //    scanner.StartScan();
            //};

            //action.BeginInvoke(ar => {
            //    ThreadHelper.EndInvokeAction(ar);
            //}, action);

            //var addList = list.Where(item => !LibraryItems.Any(i => 0 == string.Compare(i.Path, item.Path, true)));
            //var updateList = list.Where(item => null != LibraryItems.FirstOrDefault(i => ((0 == string.Compare(i.Path, item.Path, true)) && i.Level != item.Level)));
            if (0 == addList.Count() && 0 == updateList.Count())
                return;

            Func<IDictionary<LibraryItemEntity, IList<DirectoryInfo>>> function = () =>
            {
                AddLibraryItems(addList.ToList());
                UpdateLibraryItems(updateList.ToList());
                List<LibraryItemEntity> allList = new List<LibraryItemEntity>();
                allList.AddRange(addList);
                allList.AddRange(updateList);
                HistoryScanner scanner = new HistoryScanner(allList);
                scanner.StartScan();
                IList<HistoryEntity> historyList = new List<HistoryEntity>();
                foreach (KeyValuePair<LibraryItemEntity, IList<DirectoryInfo>> pair in scanner.Results)
                {
                    string defaultCategoryName = Path.GetFileName(pair.Key.Path);
                    CategoryEntity category = Categories.FirstOrDefault(item => 0 == string.Compare(defaultCategoryName, item.Name, true));
                    if (category.IsNull())
                    {
                        category = new CategoryEntity() { ID = -1, Name = defaultCategoryName };
                        if (1 == DBHelper.AddCategories(new List<CategoryEntity>() { category }))
                        {
                            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
                            {
                                categories.Add(category);
                            }));
                        }

                        foreach(DirectoryInfo scannedItem in pair.Value)
                        {
                            HistoryEntity history = new HistoryEntity() { IsDeleted = false, Name = scannedItem.Name, Path = scannedItem.FullName, Comment = string.Empty };
                            if (!category.IsNull())
                                history.CategoryIDs.Add(category.ID);
                            historyList.Add(history);
                        }
                    }
                }
                
                //var dirList = scannedFolders.Where(item => !Histories.Any(i => 0 == string.Compare(i.Path, item.FullName, true)));
                
                //foreach (var dir in dirList)
                //{
                //    string defaultCategoryName = dir.Parent.Name;
                //    CategoryEntity category = Categories.FirstOrDefault(item => 0 == string.Compare(defaultCategoryName, item.Name, true));
                //    if (category.IsNull())
                //    {
                //        category = new CategoryEntity() { ID = -1, Name = defaultCategoryName };
                //        if (1 == DBHelper.AddCategories(new List<CategoryEntity>() { category }))
                //        {
                //            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
                //            {
                //                categories.Add(category);
                //            }));
                            
                //        }
                //    }
                //    HistoryEntity history = new HistoryEntity() { IsDeleted = false, Name = dir.Name, Path = dir.FullName, Comment = string.Empty };
                //    if (!category.IsNull())
                //        history.CategoryIDs.Add(category.ID);
                //    historyList.Add(history);
                //}

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
