using HistoryManagement.Infrastructure;
using HistoryManagement.Infrastructure.UIModel;
using IDAL.Model;
using Microsoft.Practices.ServiceLocation;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
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
            One = 1,
            [Description("2")]
            Two,
            [Description("3")]
            Three,
            [Description("Unlimited")]
            Unlimited = -1
        }
        private IEventAggregator eventAggregator;
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

        private ScanRank selectedScanRank = ScanRank.One;
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

        private IList<LibraryItemEntity> newAddedList = new List<LibraryItemEntity>();
        private IList<LibraryItemEntity> updatedList = new List<LibraryItemEntity>();

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
            newAddedList.Clear();
            updatedList.Clear();
            DataManager.Instance.LibraryItems.ToList().ForEach(item => CacheLibraryList.Add(item));
        }

        private void SettingBaseTabItem_Loaded(object sender, RoutedEventArgs e)
        {
            this.eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            GlobalCommands.SaveAllSettingsCommand.RegisterCommand(SaveCommand);
        }

        private void SettingBaseTabItem_Unloaded(object sender, RoutedEventArgs e)
        {
            GlobalCommands.SaveAllSettingsCommand.UnregisterCommand(SaveCommand);
        }

        private void OnSave(object obj)
        {
            DataManager.Instance.StartScanAndUpdateDB(newAddedList, updatedList);
            if (!this.eventAggregator.IsNull())
                this.eventAggregator.GetEvent<SubSettingSavedEvent>().Publish(new SubSettingArgs() { SubSettingName = this.GetType().Name });
        }

        private void OnBrowse(object obj)
        {
            TargetPath = InfrastructureUtility.BrowseFolder(ResourceProvider.LoadString("IDS_SETTINGS_LIBRARY_BROWSEINFO"), string.Empty);
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
                newAddedList.Add(new LibraryItemEntity() { Path = TargetPath, Level = (int)SelectedScanRank });
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
                    item.Level = (int)SelectedScanRank;
                    updatedList.Add(item);
                }
            }
            TargetPath = string.Empty;
        }

        
    }
}
