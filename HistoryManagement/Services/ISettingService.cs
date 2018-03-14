using FileExplorer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HistoryManagement.Services
{
    public interface ISettingService
    {
        void SaveLibrary(IList<IFolder> selectedFolders, Action callback);
    }
}
