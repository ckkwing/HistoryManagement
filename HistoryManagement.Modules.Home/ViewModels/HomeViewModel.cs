using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HistoryManagement.Modules.Home.ViewModels
{
    [Export(typeof(HomeViewModel))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class HomeViewModel : BindableBase
    {
        private string name = "HomeViewModel";

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
                RaisePropertyChanged("Name");
            }
        }

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

        public HomeViewModel()
        {
            this.ConfirmationRequest = new InteractionRequest<IConfirmation>();
            this.SubmitCommand = new DelegateCommand<object>(this.OnSubmit, obj => { return true; });
        }

        private void OnSubmit(object obj)
        {
            this.ConfirmationRequest.Raise(
        new Confirmation { Content = "Confirmation Message", Title = "Confirmation" },
        c => { InteractionResultMessage = c.Confirmed ? "The user accepted." : "The user cancelled."; });
        }
    
    }
}
