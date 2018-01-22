using HistoryManagement.Infrastructure;
using HistoryManagement.Infrastructure.Events;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HistoryManagement.Modules.Header.ViewModels
{
    [Export(typeof(MenuViewModel))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class MenuViewModel : ViewModelBase
    {
        public ICommand OpenSettingWindowCommand { get; private set; }
        [Import]
        public IEventAggregator EventAggregator;

        public MenuViewModel()
        {
            this.OpenSettingWindowCommand = new DelegateCommand<object>(OnOpenSettingWindow);
        }

        private void OnOpenSettingWindow(object obj)
        {
            EventAggregator.GetEvent<OpenSettingEvent>().Publish(new SettingEventArgs());
        }
    }
}
