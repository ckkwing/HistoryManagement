using Prism.Interactivity.InteractionRequest;
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
using System.Windows.Shapes;
using Theme.CustomControl.Dialog;
using Utilities.Extension;

namespace HistoryManagement.Settings
{
    /// <summary>
    /// Interaction logic for SettingWindow.xaml
    /// </summary>
    public partial class SettingWindow : CommonDialog
    {
        SettingWindowViewModel ViewModel
        {
            get
            {
                return this.DataContext as SettingWindowViewModel;
            }
            set
            {
                this.DataContext = value;
            }
        }

        public SettingWindow(Window owner = null)
            :base(owner)
        {
            InitializeComponent();
            this.DataContext = new SettingWindowViewModel();
        }

        private void settingWindow_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void settingWindow_Unloaded(object sender, RoutedEventArgs e)
        {
            if (!ViewModel.IsNull())
                ViewModel.Dispose();
        }
    }
}
