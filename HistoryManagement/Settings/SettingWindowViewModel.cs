using FileExplorer.Model;
using HistoryManagement.Infrastructure;
using HistoryManagement.Services;
using Microsoft.Practices.ServiceLocation;
using Prism.Commands;
using Prism.Events;
using Prism.Interactivity.InteractionRequest;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Utilities.Common;
using Utilities.Extension;

namespace HistoryManagement.Settings
{
    public class SettingWindowViewModel : ViewModelBase, IConfirmation, IInteractionRequestAware
    {
        //private IEventAggregator eventAggregator;
        public IList<IFolder> SelectedFileList { get; set; }
        private int selectedIndex = 0;
        public int SelectedIndex
        {
            get
            {
                return selectedIndex;
            }

            set
            {
                selectedIndex = value;
                RaisePropertyChanged("SelectedIndex");
            }
        }
        public ICommand CancelCommand { get; private set; }

        #region IConfirmation
        public bool Confirmed { get; set; }
        public string Title { get; set; }
        public object Content { get; set; }
        #endregion

        #region IInteractionRequestAware
        public INotification Notification { get; set; }

        public Action FinishInteraction { get; set; }
        #endregion

        public SettingWindowViewModel()
        {
            this.Title = ResourceProvider.LoadString("IDS_SETTING");
            CancelCommand = new DelegateCommand<object>(OnCancel, CanExcute);
            //this.eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            GlobalCommands.SaveAllSettingsCommand.SetCallback(OnSettingSaved);
        }

        private void OnSettingSaved(object obj)
        {
            this.Confirmed = true;
            this.FinishInteraction();
        }

        private void OnCancel(object obj)
        {
            this.Confirmed = false;
            this.FinishInteraction();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
