using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Utilities.Settings;

namespace HistoryManagement
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ResourceDictionary lanRes = LanguageSetting.LoadLanguageResource();
            Application.Current.Resources.MergedDictionaries.Add(lanRes);
            ResourceDictionary localizedCommonRes = LanguageSetting.LoadLocalizedCommonDictionary();
            Application.Current.Resources.MergedDictionaries.Add(localizedCommonRes);

            Bootstrapper bootstrapper = new Bootstrapper();
            bootstrapper.Run();

            this.ShutdownMode = ShutdownMode.OnMainWindowClose;
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
        }
    }
}
