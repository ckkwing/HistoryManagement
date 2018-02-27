using HistoryManagement.Infrastructure;
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
using Utilities.Extension;

namespace HistoryManagement.Settings
{
    public class SettingWindowViewModel : ViewModelBase
    {
        private IEventAggregator eventAggregator;
        private int receivedSavedEvent = 0;
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

        public InteractionRequest<Notification> CloseRequest { get; set; }
        public ICommand CancelCommand { get; private set; }

        public SettingWindowViewModel()
        {
            CancelCommand = new DelegateCommand<object>(OnCancel, CanExcute);
            this.eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            if (!this.eventAggregator.IsNull())
                this.eventAggregator.GetEvent<SubSettingSavedEvent>().Subscribe(OnSubSettingSaved, ThreadOption.UIThread);
            this.CloseRequest = new InteractionRequest<Notification>();
        }

        private void OnSubSettingSaved(SubSettingArgs obj)
        {
            lock(this)
            {
                receivedSavedEvent++;
                if (2 == receivedSavedEvent)
                    this.CloseRequest.Raise(new Notification());
            }
        }

        private void OnCancel(object obj)
        {
            this.CloseRequest.Raise(new Notification());
        }

        protected override void Dispose(bool disposing)
        {
            if (!this.eventAggregator.IsNull())
                this.eventAggregator.GetEvent<SubSettingSavedEvent>().Unsubscribe(OnSubSettingSaved);
            base.Dispose(disposing);
        }
    }
}
