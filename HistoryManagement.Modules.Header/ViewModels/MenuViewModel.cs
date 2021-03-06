﻿using HistoryManagement.Infrastructure;
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
        public ICommand ViewByCategoryCommand { get; private set; }
        public ICommand ViewByPathCommand { get; private set; }
        public ICommand RefreshLibraryCommand { get; private set; }
        [Import]
        public IEventAggregator EventAggregator;

        public MenuViewModel()
        {
            OpenSettingWindowCommand = new DelegateCommand<object>(OnOpenSettingWindow);
            ViewByCategoryCommand = new DelegateCommand<object>(OnViewByCategory);
            ViewByPathCommand = new DelegateCommand<object>(OnViewByPath);
            RefreshLibraryCommand = new DelegateCommand<object>(OnRefreshLibrary);
        }

        private void OnRefreshLibrary(object obj)
        {
            EventAggregator.GetEvent<MenuViewEvent>().Publish(new MenuViewEventArgs() { MenuViewType = MenuViewType.File_RefreshLibrary });
        }

        private void OnViewByPath(object obj)
        {
            EventAggregator.GetEvent<MenuViewEvent>().Publish(new MenuViewEventArgs() { MenuViewType = MenuViewType.View_Path });
        }

        private void OnViewByCategory(object obj)
        {
            EventAggregator.GetEvent<MenuViewEvent>().Publish(new MenuViewEventArgs() { MenuViewType = MenuViewType.View_Category });
        }

        private void OnOpenSettingWindow(object obj)
        {
            EventAggregator.GetEvent<OpenSettingEvent>().Publish(new SettingEventArgs());
        }
    }
}
