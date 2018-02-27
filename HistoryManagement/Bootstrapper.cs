using HistoryManagement.Modules.Home;
using Prism.Mef;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Prism.Regions;
using Prism.Regions.Behaviors;
using Prism.Mef.Regions.Behaviors;
using HistoryManagement.Modules.Header;
using HistoryManagement.Modules.Navigation;
using Microsoft.Practices.ServiceLocation;

namespace HistoryManagement
{
    class Bootstrapper : MefBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            if (ServiceLocator.IsLocationProviderSet)
                return ServiceLocator.Current.GetInstance<Shell>();
            return this.Container.GetExportedValue<Shell>();
        }

        protected override void InitializeShell()
        {
            Application.Current.MainWindow = Shell as Shell;
            Application.Current.MainWindow.Show();
        }

        protected override void ConfigureAggregateCatalog()
        {
            this.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(Bootstrapper).Assembly));
            this.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(HomeModule).Assembly));
            this.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(HeaderModule).Assembly));
            this.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(NavigationModule).Assembly));
        }

        //protected override IRegionBehaviorFactory ConfigureDefaultRegionBehaviors()
        //{
        //    var factory = base.ConfigureDefaultRegionBehaviors();

        //    factory.AddIfMissing(MefAutoPopulateRegionBehavior.BehaviorKey, typeof(MefAutoPopulateRegionBehavior));

        //    return factory;
        //}
    }
}
