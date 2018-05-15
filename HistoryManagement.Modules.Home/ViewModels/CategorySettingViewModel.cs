using HistoryManagement.Infrastructure;
using HistoryManagement.Infrastructure.UIModel;
using IDAL.Model;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Utilities.Common;
using Utilities.Extension;

namespace HistoryManagement.Modules.Home.ViewModels
{
    public class CategorySettingViewModel : ViewModelBase, IConfirmation, IInteractionRequestAware
    {
        private string newCategory = string.Empty;
        public string NewCategory
        {
            get
            {
                return newCategory;
            }

            set
            {
                newCategory = value;
                RaisePropertyChanged("NewCategory");
            }
        }

        private IList<UIHistoryEntity> items;
        public IList<UIHistoryEntity> Items
        {
            get
            {
                return items;
            }

            set
            {
                items = value;
            }
        }

        private ObservableCollection<UICategoryEntity> existCategories = new ObservableCollection<UICategoryEntity>();
        public ObservableCollection<UICategoryEntity> ExistCategories
        {
            get
            {
                return existCategories;
            }

            set
            {
                existCategories = value;
            }
        }

        private ObservableCollection<string> newCategories = new ObservableCollection<string>();
        public ObservableCollection<string> NewCategories
        {
            get
            {
                return newCategories;
            }

            set
            {
                newCategories = value;
            }
        }

        #region IConfirmation
        public bool Confirmed { get; set; }
        public string Title { get; set; } = ResourceProvider.LoadString("IDS_SETTING_CATEGORY_TITLE");
        public object Content { get; set; } = "Content";
        #endregion

        #region IInteractionRequestAware
        public INotification Notification { get; set; }

        public Action FinishInteraction { get; set; }
        #endregion

        public ICommand SaveCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }
        public ICommand AddNewCategoryCommand { get; private set; }
        public ICommand RemoveNewCategoryCommand { get; private set; }

        public CategorySettingViewModel(IList<UIHistoryEntity> items)
        {
            this.Items = items;
            SaveCommand = new DelegateCommand<object>(OnSave, CanExcute);
            CancelCommand = new DelegateCommand<object>(OnCancel, CanExcute);
            AddNewCategoryCommand = new DelegateCommand<object>(OnAddNewCategory, CanExcute);
            RemoveNewCategoryCommand = new DelegateCommand<object>(OnRemoveNewCategory, CanExcute);
            foreach (CategoryEntity category in DataManager.Instance.Categories)
            {
                ExistCategories.Add(UICategoryEntity.CreateFrom(category));
            }
        }

        private void OnRemoveNewCategory(object obj)
        {
            NewCategories.Remove(obj.ToString());
        }

        private void OnAddNewCategory(object obj)
        {
            if (string.IsNullOrEmpty(NewCategory) && obj.IsNull())
                return;

            string newCategory = obj.IsNull() ? NewCategory : (obj as UICategoryEntity).Name;
            if (!NewCategories.FirstOrDefault(category => 0 == string.Compare(newCategory, category, true)).IsNull())
            {
                MessageBox.Show($"Category \"{newCategory}\" exist!");
                return;
            }

            NewCategories.Add(newCategory);
            NewCategory = string.Empty;
        }

        private void OnSave(object obj)
        {
            this.Confirmed = true;
            this.FinishInteraction();
        }

        private void OnCancel(object obj)
        {
            this.Confirmed = false;
            this.FinishInteraction();
        }
    }
}
