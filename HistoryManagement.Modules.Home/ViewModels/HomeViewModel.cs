using HistoryManagement.Infrastructure;
using HistoryManagement.Infrastructure.Events;
using HistoryManagement.Infrastructure.UIModel;
using IDAL.Model;
using Prism.Commands;
using Prism.Events;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using Utilities.Extension;

namespace HistoryManagement.Modules.Home.ViewModels
{
    [Export(typeof(HomeViewModel))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class HomeViewModel : ViewModelBase
    {
        public IEventAggregator eventAggregator;
        private ContentSelectionEventArgs currentContentSelectionArgs;
        public ICommand SubmitCommand { get; private set; }
        public InteractionRequest<IConfirmation> OpenCategorySettingRequest { get; private set; } = new InteractionRequest<IConfirmation>();
        private string interactionResultMessage = string.Empty;
        public string InteractionResultMessage
        {
            get
            {
                return interactionResultMessage;
            }
            private set
            {
                interactionResultMessage = value;
                RaisePropertyChanged("InteractionResultMessage");
            }
        }


        private string searchContent = string.Empty;
        public string SearchContent
        {
            get
            {
                return searchContent;
            }

            set
            {
                if (0 != string.Compare(searchContent, value, false))
                {
                    searchContent = value;
                    RaisePropertyChanged("SearchContent");
                    if (string.IsNullOrEmpty(searchContent))
                    {
                        SetCurrentUIFilter();
                    }
                    else
                    {
                        if (UIHistories.CanFilter)
                        {
                            switch(currentContentSelectionArgs.SelectionType)
                            {
                                case SelectionType.Category:
                                    UIHistories.Filter = new Predicate<object>(GetSearchedResultInCategory);
                                    break;
                                case SelectionType.Library:
                                    UIHistories.Filter = new Predicate<object>(GetSearchedResultInPath);
                                    break;
                            }
                            UIHistories.Filter = new Predicate<object>(GetSearchedResultInCategory);
                        }
                    }
                }
                ResultCount = UIHistories.Cast<UIHistoryEntity>().Count();
            }
        }

        private int resultCount = 0;
        public int ResultCount
        {
            get
            {
                return resultCount;
            }

            set
            {
                resultCount = value;
                RaisePropertyChanged("ResultCount");
            }
        }

        private ICollectionView uiHistories;
        public ICollectionView UIHistories
        {
            get
            {
                if (uiHistories == null)
                {
                    uiHistories = CollectionViewSource.GetDefaultView(DataManager.Instance.UIHistories);
                }
                return uiHistories;
            }
        }

        public ICommand OpenLocationCommand { get; private set; } 
        public ICommand SetCategoryCommand { get; private set; }

        
        [ImportingConstructor]
        public HomeViewModel(IEventAggregator eventAggregator)
        {
            SubmitCommand = new DelegateCommand<object>(this.OnSubmit, obj => { return true; });
            OpenLocationCommand = new DelegateCommand<UIHistoryEntity>(OnOpenLocation, CanExcute);
            SetCategoryCommand = new DelegateCommand(OnSetCategory, CanExcute);
            this.eventAggregator = eventAggregator;
            if (!this.eventAggregator.IsNull())
                this.eventAggregator.GetEvent<ContentSelectionEvents>().Subscribe(OnContentSelected, ThreadOption.UIThread);
            UIHistories.Filter = new Predicate<object>(GetNone);
            ResultCount = UIHistories.Cast<UIHistoryEntity>().Count();
        }

        ~HomeViewModel()
        {
            if (!this.eventAggregator.IsNull())
                this.eventAggregator.GetEvent<ContentSelectionEvents>().Unsubscribe(OnContentSelected);
        }

        private void OnOpenLocation(UIHistoryEntity history)
        {
            if (null == history )
                return;
            System.IO.FileInfo file = new System.IO.FileInfo(history.Path);
            if (file.IsNull())
                return;
            Process.Start("explorer.exe", file.DirectoryName);
        }

        private void OnSetCategory()
        {
            OpenCategorySettingRequest.Raise(new CategorySettingViewModel(UIHistories.SourceCollection.Cast<UIHistoryEntity>().Where(item => item.IsSelected).ToList()), openCategorySetting => {
                ;
            });
        }

        private void OnContentSelected(ContentSelectionEventArgs obj)
        {
            currentContentSelectionArgs = obj;

            switch (currentContentSelectionArgs.SelectionType)
            {
                case SelectionType.Category:
                    {
                        //Action action = () =>
                        //{
                        //    RunOnUIThreadAsync(() =>
                        //    {
                        //        Histories.Clear();
                        //    });
                        //    int i = 0;
                        //    List<HistoryEntity> needRefreshedHistories = new List<HistoryEntity>();
                        //    foreach (HistoryEntity history in DataManager.Instance.Histories)
                        //    {
                        //        if (!currentContentSelectionArgs.UICategoryEntity.IsRootAll)
                        //        {
                        //            if (!history.CategoryIDs.Contains(currentContentSelectionArgs.UICategoryEntity.ID))
                        //                continue;
                        //        }
                        //        needRefreshedHistories.Add(history);
                        //        i++;
                        //        if (100 == i)
                        //        {
                        //            try
                        //            {
                        //                RunOnUIThreadAsync(() =>
                        //                {
                        //                    Histories.AddRange(needRefreshedHistories);
                        //                    needRefreshedHistories.Clear();
                        //                });
                        //            }
                        //            catch (Exception e)
                        //            {
                        //                ;
                        //            }
                        //            i = 0;
                        //        }
                        //    }
                        //    RunOnUIThreadAsync(() =>
                        //    {
                        //        Histories.AddRange(needRefreshedHistories);
                        //        needRefreshedHistories.Clear();
                        //    });
                        //};

                        //action.BeginInvoke((ar) =>
                        //{
                        //    action.EndInvoke(ar);
                        //}, action);

                        SetCurrentUIFilter();
                    }
                    break;
                case SelectionType.Library:
                    {
                        SetCurrentUIFilter();
                    }
                    break;

            }

        }

        private void SetCurrentUIFilter()
        {
            if (UIHistories.CanFilter)
            {
                switch (currentContentSelectionArgs.SelectionType)
                {
                    case SelectionType.Category:
                        {
                            if (currentContentSelectionArgs.UICategoryEntity.IsRootAll)
                                UIHistories.Filter = new Predicate<object>(GetAll);
                            else
                                UIHistories.Filter = new Predicate<object>(GetCategory);
                        }
                        break;
                    case SelectionType.Library:
                        {
                            UIHistories.Filter = new Predicate<object>(obj => {
                                if (currentContentSelectionArgs.IsNull())
                                    return false;
                                UIHistoryEntity item = obj as UIHistoryEntity;
                                if (item.IsNull())
                                    return false;
                                if (item.Path.StartWithIgnoreCase(currentContentSelectionArgs.LibraryItemEntity.Path))
                                    return true;
                                return false;
                            });
                        }
                        break;
                }
              

                SearchContent = string.Empty;
                //ResultCount = UIHistories.Cast<HistoryEntity>().Count();
            }
        }

        private bool GetSearchedResultInPath(object obj)
        {
            if (currentContentSelectionArgs.IsNull())
                return false;
            UIHistoryEntity item = obj as UIHistoryEntity;
            if (item.IsNull())
                return false;
            if (item.Path.StartWithIgnoreCase(currentContentSelectionArgs.LibraryItemEntity.Path) && item.Path.IndexOf(searchContent, StringComparison.OrdinalIgnoreCase) >= 0)
                return true;
            return false;
        }

        private bool GetNone(object obj)
        {
            return false;
        }

        private bool GetAll(object obj)
        {
            return true;
        }
        private bool GetCategory(object obj)
        {
            if (currentContentSelectionArgs.IsNull())
                return false;
            UIHistoryEntity item = obj as UIHistoryEntity;
            if (item.IsNull())
                return false;
            if (item.CategoryIDs.Contains(currentContentSelectionArgs.UICategoryEntity.ID))
                return true;
            return false;
        }

        private bool GetSearchedResultInCategory(object obj)
        {
            if (currentContentSelectionArgs.IsNull())
                return false;
            UIHistoryEntity item = obj as UIHistoryEntity;
            if (item.IsNull())
                return false;
            if (currentContentSelectionArgs.UICategoryEntity.IsRootAll)
            {
                if (item.Path.IndexOf(searchContent, StringComparison.OrdinalIgnoreCase) >= 0)
                    return true;
            }
            else
            {
                if (item.CategoryIDs.Contains(currentContentSelectionArgs.UICategoryEntity.ID) && item.Path.IndexOf(searchContent, StringComparison.OrdinalIgnoreCase) >= 0)
                    return true;
            }
            return false;
        }

        private void OnSubmit(object obj)
        {
        //    this.ConfirmationRequest.Raise(
        //new Confirmation { Content = "Confirmation Message", Title = "Confirmation" },
        //c => { InteractionResultMessage = c.Confirmed ? "The user accepted." : "The user cancelled."; });
        }

    }
}
