using HistoryManagement.Infrastructure;
using IDAL.Model;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using Utilities;
using Utilities.Common;
using Utilities.Extension;

namespace HistoryManagement.Settings.SubSettingItems
{
    /// <summary>
    /// Interaction logic for AddLibrarySettingTabItem.xaml
    /// </summary>
    public partial class AddLibrarySettingTabItem : SettingBaseTabItem
    {
        public enum ScanRank
        {
            [Description("1")]
            One,
            [Description("2")]
            Two,
            [Description("3")]
            Three,
            [Description("Unlimited")]
            Unlimited = -1
        }

        private string targetPath = string.Empty;
        public string TargetPath
        {
            get
            {
                return targetPath;
            }

            set
            {
                targetPath = value;
                OnPropertyChanged("TargetPath");
            }
        }

        public IDictionary<ScanRank, string> ScanRanks
        {
            get
            {
                var r = new Dictionary<ScanRank, string>();
                foreach (ScanRank item in Enum.GetValues(typeof(ScanRank)))
                    r.Add(item, EnumHelper.GetEnumDesc(item));
                return r;
            }
        }

        private ScanRank selectedScanRank = ScanRank.Unlimited;
        public ScanRank SelectedScanRank
        {
            get
            {
                return selectedScanRank;
            }

            set
            {
                selectedScanRank = value;
                OnPropertyChanged("SelectedScanRank");
            }
        }

        private ObservableCollection<LibraryItemEntity> cacheLibraryList = new ObservableCollection<LibraryItemEntity>();
        public ObservableCollection<LibraryItemEntity> CacheLibraryList
        {
            get
            {
                return cacheLibraryList;
            }

            set
            {
                cacheLibraryList = value;
            }
        }

        public ICommand BrowseCommand { get; private set; }
        public ICommand AddLibraryCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }

        public AddLibrarySettingTabItem()
        {
            InitializeComponent();
            this.DataContext = this;

            this.BrowseCommand = new DelegateCommand<object>(OnBrowse, obj => { return true; });
            this.AddLibraryCommand = new DelegateCommand<object>(OnAddLibrary, obj => { return true; });
            this.SaveCommand = new DelegateCommand<object>(OnSave, obj => { return true; });

            DataManager.Instance.LibraryItems.ToList().ForEach(item => CacheLibraryList.Add(item));
        }

        private void SettingBaseTabItem_Loaded(object sender, RoutedEventArgs e)
        {
            GlobalCommands.SaveAllSettingsCommand.RegisterCommand(SaveCommand);
        }

        private void SettingBaseTabItem_Unloaded(object sender, RoutedEventArgs e)
        {
            GlobalCommands.SaveAllSettingsCommand.UnregisterCommand(SaveCommand);
        }

        private void OnSave(object obj)
        {
            MessageBox.Show("AddLibrarySettingTabItem onSave");
        }

        private void OnBrowse(object obj)
        {
            TargetPath = Infrastructure.InfrastructureUtility.BrowseFolder(ResourceProvider.LoadString("IDS_SETTINGS_LIBRARY_BROWSEINFO"), string.Empty);
        }

        private void OnAddLibrary(object obj)
        {
            LibraryItemEntity item = (from library in CacheLibraryList
                                      where (0 == string.Compare(library.Path, TargetPath, true))
                                      select library).FirstOrDefault<LibraryItemEntity>();
            if (item.IsNull())
            {
                //Add to library
                MessageBox.Show("new");
                CacheLibraryList.Add(new LibraryItemEntity() { Path = TargetPath, Level = (int)SelectedScanRank });
            }
            else
            {
                if (item.Level == (int)SelectedScanRank)
                {
                    MessageBox.Show("Exist same");
                }
                else
                {
                    MessageBox.Show("Exist need update");
                }
            }
            TargetPath = string.Empty;
        }

        
    }
}
