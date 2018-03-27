using HistoryManagement.Modules.Navigation.ViewModels;
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

namespace HistoryManagement.Modules.Navigation.Views
{
    /// <summary>
    /// Interaction logic for PathNavigationView.xaml
    /// </summary>
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class PathNavigationView : UserControl
    {
        [Import]
        PathNavigationViewModel ViewModel
        {
            get
            {
                return this.DataContext as PathNavigationViewModel;
            }
            set
            {
                this.DataContext = value;
            }
        }

        public PathNavigationView()
        {
            InitializeComponent();
        }
    }
}
