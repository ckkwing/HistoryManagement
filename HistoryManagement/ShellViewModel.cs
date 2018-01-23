using HistoryManagement.Infrastructure;
using HistoryManagement.Infrastructure.Events;
using HistoryManagement.Settings;
using Prism.Events;
using Prism.Interactivity.InteractionRequest;
using Prism.Modularity;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HistoryManagement
{
    [Export]
    public class ShellViewModel : ViewModelBase, IPartImportsSatisfiedNotification
    {
        [Import(AllowRecomposition = false)]
        public IModuleManager ModuleManager;
        public IEventAggregator EventAggregator;

        public InteractionRequest<INotification> OpenSettingWindowRequest { get; private set; }

        [ImportingConstructor]
        public ShellViewModel(IEventAggregator eventAggregator)
        {
            this.EventAggregator = eventAggregator;
            this.EventAggregator.GetEvent<OpenSettingEvent>().Subscribe(OnOpenSettingWindow, ThreadOption.UIThread);

            this.OpenSettingWindowRequest = new InteractionRequest<INotification>();
        }

        ~ShellViewModel()
        {
            this.EventAggregator.GetEvent<OpenSettingEvent>().Unsubscribe(OnOpenSettingWindow);
        }

        private void OnOpenSettingWindow(SettingEventArgs obj)
        {
            //    this.ConfirmationRequest.Raise(
            //new Confirmation { Content = "Confirmation Message", Title = "Confirmation" },
            //c => { InteractionResultMessage = c.Confirmed ? "The user accepted." : "The user cancelled."; });

            //this.OpenSettingWindowRequest.Raise(new Notification() { Title = "Setting Window", Content = "This is the setting window" }, OnSettingWindowClosed);

            SettingWindow win = new SettingWindow();
            win.ShowDialog();
        }

        //private void OnSettingWindowClosed(INotification obj)
        //{
        //    ;
        //}

        #region IPartImportsSatisfiedNotification Members
        public void OnImportsSatisfied()
        {
            ModuleManager.LoadModuleCompleted += ModuleManager_LoadModuleCompleted;
        }

        void ModuleManager_LoadModuleCompleted(object sender, LoadModuleCompletedEventArgs e)
        {
            NLogger.LogHelper.UILogger.DebugFormat("ModuleManager_LoadModuleCompleted, Module name: {0}", e.ModuleInfo.ModuleName);
        }
        #endregion
    }
}
