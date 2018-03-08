using HistoryManagement.Infrastructure;
using HistoryManagement.Infrastructure.Events;
using HistoryManagement.Modules.Navigation.Views;
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

namespace HistoryManagement.Modules.Navigation
{
    [ModuleExport(typeof(NavigationModule), InitializationMode = InitializationMode.WhenAvailable)]
    public class NavigationModule : IModule
    {
        private readonly IRegionViewRegistry regionViewRegistry;
        private readonly IEventAggregator eventAggregator;
        private readonly IRegionManager regionManager;

        [ImportingConstructor]
        public NavigationModule(IRegionViewRegistry registry, IEventAggregator eventAggregator, IRegionManager regionManager)
        {
            this.regionViewRegistry = registry;
            this.regionManager = regionManager;
            this.eventAggregator = eventAggregator;
            this.eventAggregator.GetEvent<MenuViewEvent>().Subscribe(OnViewChanged, ThreadOption.UIThread);
        }
        public void Initialize()
        {
            //regionViewRegistry.RegisterViewWithRegion(RegionNames.NavigationRegion, typeof(CategoryNavigationView));
        }

        private void OnViewChanged(MenuViewEventArgs obj)
        {
            if (null == regionManager)
                throw new ArgumentNullException("IRegionManager");
            IRegion region = regionManager.Regions[RegionNames.NavigationRegion];
            if (null == region)
                throw new ArgumentNullException("IRegion");

            Type type = typeof(CategoryNavigationView);
            switch (obj.MenuViewType)
            {
                case MenuViewType.Category:
                    type = typeof(CategoryNavigationView);
                    break;
                case MenuViewType.Path:
                    type = typeof(PathNavigationView);
                    break;
            }
            try
            {
                var sourcesView = region.GetView(type.Name);
                if (sourcesView == null)
                {
                    sourcesView = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(type);
                    region.Add(sourcesView, type.Name);
                }
            }
            catch (Exception e)
            {
                NLogger.LogHelper.UILogger.Error("AddView exception", e);
            }

            regionManager.RequestNavigate(RegionNames.NavigationRegion, new Uri(type.Name, UriKind.Relative));
        }
    }
}
