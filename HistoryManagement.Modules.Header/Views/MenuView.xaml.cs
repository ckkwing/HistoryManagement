using HistoryManagement.Modules.Header.ViewModels;
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

namespace HistoryManagement.Modules.Header.Views
{
    /// <summary>
    /// Interaction logic for MenuView.xaml
    /// </summary>
    [Export]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class MenuView : UserControl
    {
        [Import]
        MenuViewModel ViewModel
        {
            get
            {
                return this.DataContext as MenuViewModel;
            }
            set
            {
                this.DataContext = value;
            }
        }

        public MenuView()
        {
            InitializeComponent();
        }
    }
}
