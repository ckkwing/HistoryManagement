﻿using Prism.Interactivity.InteractionRequest;
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

namespace HistoryManagement.Settings
{
    /// <summary>
    /// Interaction logic for SettingWindow.xaml
    /// </summary>
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class SettingWindow : CommonDialog
    {
        [Import]
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
            
        }

    }
}
