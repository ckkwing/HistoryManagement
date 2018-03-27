using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HistoryManagement.Infrastructure.Events
{
    public class SettingEventArgs : EventArgs
    {
    }

    public class OpenSettingEvent : PubSubEvent<SettingEventArgs>
    {

    }

    public enum MenuViewType
    {
        File_RefreshLibrary,
        View_Category,
        View_Path
    }
    public class MenuViewEventArgs : EventArgs
    {
        public MenuViewType MenuViewType { get; set; }
    }
    public class MenuViewEvent : PubSubEvent<MenuViewEventArgs>
    {

    }
}
