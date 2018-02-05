using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Utilities.Extension;

namespace HistoryManagement.Infrastructure
{
    public class InfrastructureUtility
    {
        public static string BrowseFolder(string description, string selectedPath, Window owner = null)
        {
            System.Windows.Interop.HwndSource source = null;
            if (owner.IsNull())
            {
                source = PresentationSource.FromVisual(Application.Current.MainWindow) as System.Windows.Interop.HwndSource;
            }
            else
            {
                source = PresentationSource.FromVisual(owner) as System.Windows.Interop.HwndSource;
            }

            return Utilities.Utility.BrowseFolder(description, selectedPath, source);
        }
    }
}
