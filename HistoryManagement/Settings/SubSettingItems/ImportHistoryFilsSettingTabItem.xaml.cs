using HistoryManagement.Infrastructure;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Utilities.Extension;

namespace HistoryManagement.Settings.SubSettingItems
{
    /// <summary>
    /// Interaction logic for ImportHistoryFilsSettingTabItem.xaml
    /// </summary>
    public partial class ImportHistoryFilsSettingTabItem : SettingBaseTabItem
    {
        private IEventAggregator eventAggregator;
        public ICommand SaveCommand { get; private set; }
        public ImportHistoryFilsSettingTabItem()
        {
            InitializeComponent();
            this.DataContext = this;
            this.SaveCommand = new DelegateCommand<object>(OnSave, obj => { return true; });
        }

        private void OnSave(object obj)
        {
            if (!this.eventAggregator.IsNull())
                this.eventAggregator.GetEvent<SubSettingSavedEvent>().Publish(new SubSettingArgs() { SubSettingName = "" });
        }

        private void SettingBaseTabItem_Loaded(object sender, RoutedEventArgs e)
        {
            this.eventAggregator = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IEventAggregator>();
            GlobalCommands.SaveAllSettingsCommand.RegisterCommand(SaveCommand);
        }

        private void SettingBaseTabItem_Unloaded(object sender, RoutedEventArgs e)
        {
            GlobalCommands.SaveAllSettingsCommand.UnregisterCommand(SaveCommand);
        }
    }
}
