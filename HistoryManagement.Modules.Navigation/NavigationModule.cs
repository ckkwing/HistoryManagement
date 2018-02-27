using HistoryManagement.Infrastructure;
using Prism.Mef.Modularity;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HistoryManagement.Modules.Navigation
{
    [ModuleExport(typeof(NavigationModule), InitializationMode = InitializationMode.WhenAvailable)]
    public class NavigationModule : IModule
    {
        private readonly IRegionViewRegistry regionViewRegistry;

        [ImportingConstructor]
        public NavigationModule(IRegionViewRegistry registry)
        {
            regionViewRegistry = registry;
        }
        public void Initialize()
        {
            regionViewRegistry.RegisterViewWithRegion(RegionNames.NavigationRegion, typeof(Views.CategoryNavigationView));
        }
    }
}
