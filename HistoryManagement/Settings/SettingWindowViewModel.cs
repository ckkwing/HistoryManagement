using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HistoryManagement.Settings
{
    [Export(typeof(SettingWindowViewModel))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class SettingWindowViewModel
    {
        public SettingWindowViewModel()
        {
            ;
        }
    }
}
