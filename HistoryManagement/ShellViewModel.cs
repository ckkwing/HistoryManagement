using HistoryManagement.Infrastructure;
using HistoryManagement.Infrastructure.Events;
using HistoryManagement.Services;
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
        [Import]
        ISettingService SettingService;

        private readonly InteractionRequest<SettingWindowViewModel> openSettingWindowRequest;
        public IInteractionRequest OpenSettingWindowRequest
        {
            get { return this.openSettingWindowRequest; }
        }

        private bool inProgress = false;
        public bool InProgress
        {
            get
            {
                return inProgress;
            }

            private set
            {
                inProgress = value;
                RaisePropertyChanged("InProgress");
            }
        }

        [ImportingConstructor]
        public ShellViewModel(IEventAggregator eventAggregator)
        {
            this.EventAggregator = eventAggregator;
            this.EventAggregator.GetEvent<OpenSettingEvent>().Subscribe(OnOpenSettingWindow, ThreadOption.UIThread);

            this.openSettingWindowRequest = new InteractionRequest<SettingWindowViewModel>();
        }

        ~ShellViewModel()
        {
            this.EventAggregator.GetEvent<OpenSettingEvent>().Unsubscribe(OnOpenSettingWindow);
        }

        private void OnOpenSettingWindow(SettingEventArgs obj)
        {
            SettingWindowViewModel viewModel = new SettingWindowViewModel();

            this.openSettingWindowRequest.Raise(
                viewModel,
                openSetting =>
                {
                    if (openSetting.Confirmed)
                    {
                        this.InProgress = true;
                        
                        SettingService.SaveLibrary(
                            viewModel.SelectedFileList,
                            () => {
                                this.InProgress = false;
                            });
                    }
                });

             
        }

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
