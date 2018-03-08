using HistoryManagement.Infrastructure;
using HistoryManagement.Infrastructure.Events;
using HistoryManagement.Infrastructure.UIModel;
using IDAL.Model;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Utilities.Common;
using Utilities.Extension;

namespace HistoryManagement.Modules.Navigation.ViewModels
{
    [Export(typeof(CategoryNavigationViewModel))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class CategoryNavigationViewModel : ViewModelBase
    {
        private IEventAggregator eventAggregator;
        private UICategoryEntity root = new UICategoryEntity() { ID = -1, Name = ResourceProvider.LoadString("IDS_ALL"), Description = ResourceProvider.LoadString("IDS_ALL"), IsRootAll = true };
        public UICategoryEntity Root
        {
            get
            {
                return root;
            }

            set
            {
                root = value;
                RaisePropertyChanged("Root");
            }
        }

        public ICollectionView UICategories { get; private set; }

        [ImportingConstructor]
        public CategoryNavigationViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            if (!this.eventAggregator.IsNull())
            {
                eventAggregator.GetEvent<DBDataRefreshedEvents>().Subscribe(OnDBDataRefreshed, ThreadOption.UIThread);
            }
            ComposeData();
            UICategories = new CollectionView(new List<UICategoryEntity>() { Root });
        }

        private void OnDBDataRefreshed(DBDataRefreshedEventEventArgs args)
        {
            switch(args.DBDataType)
            {
                case DBDataType.All:
                case DBDataType.Category:
                    ComposeData();
                    break;
            }
        }

        private void ComposeData()
        {
            Root.Children.Clear();
            foreach (CategoryEntity item in DataManager.Instance.Categories)
            {
                UICategoryEntity uiItem = UICategoryEntity.CreateFrom(item);
                if (uiItem.IsNull())
                    continue;
                Root.Children.Add(uiItem);
            }
            Root.IsSelected = true;
            //Root.Children = CollectionViewSource.GetDefaultView(source);
            //Root.Children.Refresh();
        }
    }
}
