using HistoryManagement.Infrastructure;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HistoryManagement.Settings
{
    public class SettingWindowViewModel : ViewModelBase
    {
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

        public ICommand SaveCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }

        public SettingWindowViewModel()
        {
            SaveCommand = new DelegateCommand<object>(OnSave, CanExcute);
            CancelCommand = new DelegateCommand<object>(OnCancel, CanExcute);

            this.CloseRequest = new InteractionRequest<Notification>();
        }

        private void OnCancel(object obj)
        {
            this.CloseRequest.Raise(new Notification());
        }

        private void OnSave(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
