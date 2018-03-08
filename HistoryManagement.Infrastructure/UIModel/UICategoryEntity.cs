using HistoryManagement.Infrastructure.Events;
using IDAL.Model;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Extension;

namespace HistoryManagement.Infrastructure.UIModel
{
    public class UICategoryEntity : CategoryEntity
    {
        private bool isRootAll = false;
        public bool IsRootAll
        {
            get
            {
                return isRootAll;
            }

            set
            {
                isRootAll = value;
            }
        }

        private bool isSelected = false;
        public bool IsSelected
        {
            get
            {
                return isSelected;
            }

            set
            {
                if (isSelected == value)
                    return;
                isSelected = value;
                RaisePropertyChanged("IsSelected");
                if (isSelected)
                    PublishSelectedEvent();
            }
        }


        //public ICollectionView Children { get; set; }
        private ObservableCollection<UICategoryEntity> children = new ObservableCollection<UICategoryEntity>();
        public ObservableCollection<UICategoryEntity> Children
        {
            get
            {
                return children;
            }

            set
            {
                children = value;
                RaisePropertyChanged("Children");
            }
        }

        public static UICategoryEntity CreateFrom(CategoryEntity category)
        {
            return new UICategoryEntity()
            {
                ID = category.ID,
                Name = category.Name,
                Description = category.Description
            };
        }

        private void PublishSelectedEvent()
        {
            IEventAggregator eventAggregator = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IEventAggregator>();
            if (!eventAggregator.IsNull())
                eventAggregator.GetEvent<ContentSelectionEvents>().Publish(new ContentSelectionEventArgs() { SelectionType = SelectionType.Category, UICategoryEntity = this });
        }
    }
}
