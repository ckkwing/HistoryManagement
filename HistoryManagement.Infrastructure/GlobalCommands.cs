using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HistoryManagement.Infrastructure
{
    public static class GlobalCommands
    {
        public static CompositeCommand SaveAllSettingsCommand = new CompositeCommand();
    }
}
