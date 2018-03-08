using HistoryManagement.Infrastructure;
using HistoryManagement.Infrastructure.Events;
using Prism.Events;
using Prism.Mef.Modularity;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HistoryManagement.Modules.Header
{
    [ModuleExport(typeof(HeaderModule), InitializationMode = InitializationMode.WhenAvailable)]
    public class HeaderModule : IModule
    {
        private readonly IRegionViewRegistry regionViewRegistry;

        [ImportingConstructor]
        public HeaderModule(IRegionViewRegistry registry)
        {
            this.regionViewRegistry = registry;
        }

        public void Initialize()
        {
            regionViewRegistry.RegisterViewWithRegion(RegionNames.HeaderRegion, typeof(Views.MenuView));
        }
    }
}
