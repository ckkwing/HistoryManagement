﻿using HistoryManagement.Modules.Home.ViewModels;
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

namespace HistoryManagement.Modules.Home.Views
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    [Export]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class HomeView : UserControl
    {
        [Import]
        HomeViewModel ViewModel
        {
            get
            {
                return this.DataContext as HomeViewModel;
            }
            set
            {
                this.DataContext = value;
            }
        }

        public HomeView()
        {
            InitializeComponent();
        }

        //private void ListViewItem_RequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
        //{
        //    e.Handled = true;
        //}
    }
}
