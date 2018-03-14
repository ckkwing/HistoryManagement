using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileExplorer.Model;
using System.ComponentModel.Composition;
using Utilities.Extension;
using IDAL.Model;
using HistoryManagement.Infrastructure;

namespace HistoryManagement.Services
{
    [Export(typeof(ISettingService))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class SettingService : ISettingService
    {
        public void SaveLibrary(IList<IFolder> selectedFolders, Action callback)
        {
            if (selectedFolders.IsNull())
                return;
            
            IList<LibraryItemEntity> list = new List<LibraryItemEntity>();
            foreach (IFolder folder in selectedFolders)
            {
                LibraryItemEntity item = new LibraryItemEntity() { Path = folder.FullPath };
                list.Add(item);
            }
            DataManager.Instance.StartScanAndUpdateDB(list, callback);
        }
    }
}
