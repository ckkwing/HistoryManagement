using Prism.Modularity;
using System;
using Prism.Mef.Modularity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Regions;
using System.ComponentModel.Composition;
using HistoryManagement.Infrastructure;

namespace HistoryManagement.Modules.Home
{
    [ModuleExport(typeof(HomeModule), InitializationMode = InitializationMode.WhenAvailable)]
    public class HomeModule : IModule
    {
        private readonly IRegionViewRegistry regionViewRegistry;

        [ImportingConstructor]
        public HomeModule(IRegionViewRegistry registry)
        {
            regionViewRegistry = registry;
        }
        public void Initialize()
        {
            regionViewRegistry.RegisterViewWithRegion(RegionNames.HomeRegion, typeof(Views.HomeView));
        }
    }
}
