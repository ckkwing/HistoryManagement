using HistoryManagement.Infrastructure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Utilities;
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

            FolderDefine.Instance.Init(string.Empty, string.Empty);
            DataManager.Instance.Init();

            Bootstrapper bootstrapper = new Bootstrapper();
            bootstrapper.Run();

            this.ShutdownMode = ShutdownMode.OnMainWindowClose;
        }

        protected override void OnExit(ExitEventArgs e)
        {
            FolderDefine.Instance.Uninit();
            DataManager.Instance.Uninit();
            base.OnExit(e);
        }
    }
}
