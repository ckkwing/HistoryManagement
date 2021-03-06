﻿using HistoryManagement.Infrastructure.Events;
using HistoryManagement.Infrastructure.Scanner;
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

        private IList<HistoryEntity> histories = new List<HistoryEntity>();
        public IEnumerable<HistoryEntity> Histories
        {
            get
            {
                return histories;
            }
        }

        public IEnumerable<UIHistoryEntity> UIHistories
        {
            get
            {
                IList<UIHistoryEntity> list = new List<UIHistoryEntity>();
                foreach (HistoryEntity entity in Histories)
                {
                    list.Add(UIHistoryEntity.Create(entity));
                }
                return list;
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
            foreach (LibraryItemEntity libraryItem in DBHelper.GetLibraryItems())
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
            catch (Exception e)
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

        public void DeleteLibraryItems(IList<LibraryItemEntity> items)
        {
            if (items.IsNull())
                return;

            DBHelper.DeleteLibraryItems(items);
        }

        #region Scanner

        public void SyncHistories(IList<LibraryItemEntity> list, Action callback)
        {
            Action action = () =>
            {
                HistoryScanner scanner = new HistoryScanner(list.ToList());
                scanner.StartScan();
                WriteToDBByScannedResult(scanner.Results);
            };

            action.BeginInvoke(ar =>
            {
                RefreshDBData();
                if (!callback.IsNull())
                    callback();
                ThreadHelper.EndInvokeAction(ar);
            }, action);
        }

        public void StartScanAndUpdateDB(IList<LibraryItemEntity> list, Action callback)
        {
            Func<IDictionary<LibraryItemEntity, IList<DirectoryInfo>>> function = () =>
            {
                var removeList = LibraryItems.Where(item => list.FirstOrDefault(i => 0 == string.Compare(i.Path, item.Path, true)).IsNull());
                var addList = list.Where(item => !LibraryItems.Any(i => 0 == string.Compare(i.Path, item.Path, true)));
                DeleteLibraryItems(removeList.ToList());


                AddLibraryItems(addList.ToList());
                HistoryScanner scanner = new HistoryScanner(addList.ToList());
                scanner.StartScan();
                WriteToDBByScannedResult(scanner.Results);

                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                {
                    RefreshDBData();
                    if (!callback.IsNull())
                        callback();
                }));

                return scanner.Results;
            };

            function.BeginInvoke(ar =>
            {
                IEnumerable<DirectoryInfo> results = ThreadHelper.EndInvokeFunc<IEnumerable<DirectoryInfo>>(ar);
            }, function);
        }

        private void WriteToDBByScannedResult(IDictionary<LibraryItemEntity, IList<DirectoryInfo>> result)
        {
            IList<HistoryEntity> addHistoryList = new List<HistoryEntity>();
            IList<HistoryEntity> removeHistoryList = new List<HistoryEntity>();
            IList<DirectoryInfo> scannedResult = new List<DirectoryInfo>();
            foreach (KeyValuePair<LibraryItemEntity, IList<DirectoryInfo>> pair in result)
            {
                foreach (DirectoryInfo scannedItem in pair.Value)
                {
                    string defaultCategoryName = scannedItem.Parent.Name;
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
                    }

                    HistoryEntity history = new HistoryEntity() { IsDeleted = false, Name = scannedItem.Name, Path = scannedItem.FullName, Comment = string.Empty, LibraryID = pair.Key.ID };
                    if (!category.IsNull())
                        history.CategoryIDs.Add(category.ID);
                    if (Histories.FirstOrDefault(item => 0 == string.Compare(item.Path, history.Path, true)).IsNull())
                        addHistoryList.Add(history);

                    scannedResult.Add(scannedItem);
                }
            }
            DBHelper.AddHistoryItems(addHistoryList);

            foreach (HistoryEntity history in this.Histories)
            {
                //if (scannedResult.FirstOrDefault(item => 0 == string.Compare(item.FullName, history.Path, true)).IsNull())
                //    removeHistoryList.Add(history);
                if (!Directory.Exists(history.Path))
                    removeHistoryList.Add(history);
            }
            DBHelper.DeleteHistoryItems(removeHistoryList);

        }


        /// <summary>
        /// Not used right now
        /// </summary>
        /// <param name="addList"></param>
        /// <param name="updateList"></param>
        public void StartScanAndUpdateDB(IList<LibraryItemEntity> addList, IList<LibraryItemEntity> updateList)
        {
            //Action action = () => {
            //    HistoryScanner scanner = new HistoryScanner(CacheLibraryList);
            //    scanner.StartScan();
            //};

            //action.BeginInvoke(ar => {
            //    ThreadHelper.EndInvokeAction(ar);
            //}, action);

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

                        foreach (DirectoryInfo scannedItem in pair.Value)
                        {
                            HistoryEntity history = new HistoryEntity() { IsDeleted = false, Name = scannedItem.Name, Path = scannedItem.FullName, Comment = string.Empty };
                            if (!category.IsNull())
                                history.CategoryIDs.Add(category.ID);
                            historyList.Add(history);
                        }
                    }
                }

                DBHelper.AddHistoryItems(historyList);
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                {
                    RefreshDBData();
                }));

                return scanner.Results;
            };

            function.BeginInvoke(ar =>
            {
                IEnumerable<DirectoryInfo> results = ThreadHelper.EndInvokeFunc<IEnumerable<DirectoryInfo>>(ar);
            }, function);
        }

        #endregion
    }
}
