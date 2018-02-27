using HistoryManagement.Infrastructure;
using HistoryManagement.Infrastructure.Events;
using IDAL.Model;
using Prism.Commands;
using Prism.Events;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
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
        public ICollectionView UIHistories { get; private set; }
        public ICommand SubmitCommand { get; private set; }
        public InteractionRequest<IConfirmation> ConfirmationRequest { get; private set; }
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

        [ImportingConstructor]
        public HomeViewModel(IEventAggregator eventAggregator)
        {
            this.ConfirmationRequest = new InteractionRequest<IConfirmation>();
            this.SubmitCommand = new DelegateCommand<object>(this.OnSubmit, obj => { return true; });
            this.eventAggregator = eventAggregator;
            if (!this.eventAggregator.IsNull())
                this.eventAggregator.GetEvent<ContentSelectionEvents>().Subscribe(OnContentSelected, ThreadOption.UIThread);
            UIHistories = CollectionViewSource.GetDefaultView(DataManager.Instance.Histories);
            //UIHistories = new ListCollectionView(DataManager.Instance.Histories);
        }

        ~HomeViewModel()
        {
            if (!this.eventAggregator.IsNull())
                this.eventAggregator.GetEvent<ContentSelectionEvents>().Unsubscribe(OnContentSelected);
        }

        private void OnContentSelected(ContentSelectionEventArgs obj)
        {
            currentContentSelectionArgs = obj;
            if (UIHistories.CanFilter)
            {
                switch(currentContentSelectionArgs.SelectionType)
                {
                    case SelectionType.Category:
                        {
                            if (currentContentSelectionArgs.UICategoryEntity.IsRootAll)
                                UIHistories.Filter = new Predicate<object>(GetAll);
                            else
                                UIHistories.Filter = new Predicate<object>(GetCategory);
                        }
                        break;
                }
            }
            
        }

        private bool GetAll(object obj)
        {
            return true;
        }
        private bool GetCategory(object obj)
        {
            if (currentContentSelectionArgs.IsNull())
                return false;
            HistoryEntity item = obj as HistoryEntity;
            if (item.IsNull())
                return false;
            if (item.CategoryIDs.Contains(currentContentSelectionArgs.UICategoryEntity.ID))
                return true;
            return false;
        }

        private void OnSubmit(object obj)
        {
            this.ConfirmationRequest.Raise(
        new Confirmation { Content = "Confirmation Message", Title = "Confirmation" },
        c => { InteractionResultMessage = c.Confirmed ? "The user accepted." : "The user cancelled."; });
        }
    
    }
}
