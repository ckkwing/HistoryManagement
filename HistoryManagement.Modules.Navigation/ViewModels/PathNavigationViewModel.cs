using HistoryManagement.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HistoryManagement.Modules.Navigation.ViewModels
{
    [Export(typeof(PathNavigationViewModel))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class PathNavigationViewModel : ViewModelBase
    {
    }
}
