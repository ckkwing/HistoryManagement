using HistoryManagement.Infrastructure;
using HistoryManagement.Infrastructure.UIModel;
using IDAL.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Utilities.Common;

namespace HistoryManagement.Modules.Navigation.ViewModels
{
    [Export(typeof(CategoryNavigationViewModel))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class CategoryNavigationViewModel : ViewModelBase
    {
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

        public CategoryNavigationViewModel()
        {
            Root.Children = CollectionViewSource.GetDefaultView(DataManager.Instance.Categories);
            UICategories = new CollectionView(new List<UICategoryEntity>() { Root });
        }
    }
}
