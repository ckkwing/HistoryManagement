using FileExplorer.Model;
using FileExplorer.ViewModel;
using HistoryManagement.Infrastructure;
using HistoryManagement.Infrastructure.Command;
using IDAL.Model;
using Microsoft.Practices.ServiceLocation;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
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
        private IList<IFolder> selectedFileList = new List<IFolder>();
        public IList<IFolder> SelectedFileList
        {
            get
            {
                return selectedFileList;
            }

            private set
            {
                selectedFileList = value;
            }
        }

        private FileExplorerViewModel treeViewModel = new FileExplorerViewModel();
        public FileExplorerViewModel TreeViewModel
        {
            get
            {
                return treeViewModel;
            }
        }

        private IEventAggregator eventAggregator;
        public IAsyncCommand SaveCommand { get; private set; }
        public ImportHistoryFilsSettingTabItem()
        {
            InitializeComponent();
            this.DataContext = this;
            SaveCommand = new AsyncDelegateCommand<object>(OnSave, obj => { return true; });
        }

        private Task OnSave(object arg)
        {
            SettingWindowViewModel viewModel = arg as SettingWindowViewModel;
            if (viewModel.IsNull())
                return Task.Factory.StartNew(() => { });
                
            return Task.Factory.StartNew(() =>
            {
                viewModel.SelectedFileList = TreeViewModel.GetCheckedFolders();
            });
        }

        private void SettingBaseTabItem_Loaded(object sender, RoutedEventArgs e)
        {
            TreeViewModel.LoadLocalExplorer();
            if (!TreeViewModel.RootFolder.IsNull() &&
                             TreeViewModel.RootFolder.IsChecked == false)
            {
                IList<string> selectedPathList = new List<string>();
                DataManager.Instance.LibraryItems.ToList().ForEach(item => selectedPathList.Add(item.Path));
                TreeViewModel.SetCheckedPaths(selectedPathList);
            }
            this.eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            GlobalCommands.SaveAllSettingsCommand.RegisterCommand(SaveCommand);
        }

        private void SettingBaseTabItem_Unloaded(object sender, RoutedEventArgs e)
        {
            GlobalCommands.SaveAllSettingsCommand.UnregisterCommand(SaveCommand);
        }
    }
}
