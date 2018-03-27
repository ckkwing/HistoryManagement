using HistoryManagement.Infrastructure;
using HistoryManagement.Infrastructure.Events;
using IDAL.Model;
using Prism.Events;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Utilities.Extension;

namespace HistoryManagement.Modules.Navigation.ViewModels
{
    [Export(typeof(PathNavigationViewModel))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class PathNavigationViewModel : ViewModelBase, IRegionMemberLifetime
    {
        private IEventAggregator eventAggregator;
        private ICollectionView uiPaths;
        public ICollectionView UIPaths
        {
            get
            {
                if (uiPaths == null)
                {
                    uiPaths = CollectionViewSource.GetDefaultView(DataManager.Instance.LibraryItems);
                }
                return uiPaths;
            }
        }

        #region IRegionMemberLifetime
        public bool KeepAlive
        {
            get
            {
                return false;
            }
        }
        #endregion 

        [ImportingConstructor]
        public PathNavigationViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            if (!this.eventAggregator.IsNull())
            {
                eventAggregator.GetEvent<DBDataRefreshedEvents>().Subscribe(OnDBDataRefreshed, ThreadOption.UIThread);
            }
            UIPaths.CurrentChanged += UIPaths_CurrentChanged;
            UIPaths.MoveCurrentToFirst();
        }

        ~PathNavigationViewModel()
        {
            UIPaths.CurrentChanged -= UIPaths_CurrentChanged;
        }

        private void UIPaths_CurrentChanged(object sender, EventArgs e)
        {
            if (!eventAggregator.IsNull())
                eventAggregator.GetEvent<ContentSelectionEvents>().Publish(new ContentSelectionEventArgs() { SelectionType = SelectionType.Library, LibraryItemEntity = UIPaths.CurrentItem as LibraryItemEntity });
        }

        private void OnDBDataRefreshed(DBDataRefreshedEventEventArgs args)
        {
            switch (args.DBDataType)
            {
                case DBDataType.All:
                case DBDataType.Library:
                    UIPaths.Refresh();
                    UIPaths.MoveCurrentToFirst();
                    break;
            }
        }

        //#region INavigationAware
        //public void OnNavigatedTo(NavigationContext navigationContext)
        //{
        //    throw new NotImplementedException();
        //}

        //public bool IsNavigationTarget(NavigationContext navigationContext)
        //{
        //    throw new NotImplementedException();
        //}

        //public void OnNavigatedFrom(NavigationContext navigationContext)
        //{
        //    throw new NotImplementedException();
        //}
        //#endregion
    }
}
