using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HistoryManagement.Settings
{
    public class SubSettingArgs : EventArgs
    {
        public string SubSettingName { get; set; }
    }

    public class SubSettingSavedEvent : PubSubEvent<SubSettingArgs>
    {

    }
}
