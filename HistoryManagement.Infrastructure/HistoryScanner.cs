using HistoryManagement.Infrastructure.UIModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Extension;

namespace HistoryManagement.Infrastructure
{
    public class HistoryScanner
    {
        /// <summary>
        /// Consider as history items
        /// </summary>
        private IList<DirectoryInfo> results = new List<DirectoryInfo>();
        public IEnumerable<DirectoryInfo> Results
        {
            get
            {
                return results;
            }
        }

        private IList<string> categories = new List<string>();
        public IEnumerable<string> Categories
        {
            get
            {
                return categories;
            }
        }

        private IList<UILibraryItemEntity> entityToScan = null;
        public HistoryScanner(IList<UILibraryItemEntity> entityToScan)
        {
            this.entityToScan = entityToScan;
        }

        public void StartScan()
        {
            foreach(UILibraryItemEntity entity in entityToScan)
            {
                if (!Directory.Exists(entity.Path))
                    break;
                DirectoryInfo dir = new DirectoryInfo(entity.Path);
                int currentRank = 0;
                int rank = entity.Level == -1 ? 10000000 : entity.Level;
                ScanDirectory(dir, rank, ref results, ref currentRank);
                categories.Add(dir.Name);
            }
        }

        private void ScanDirectory(DirectoryInfo directInfo, int rank, ref IList<DirectoryInfo> results, ref int currentRank)
        {
            if (directInfo.IsNull() || rank <= 0 || currentRank == rank)
                return;

            DirectoryInfo[] directories = directInfo.GetDirectories();
            currentRank++;
            if (directories.IsNull() || 0 == directories.Length)
                return;

            for (int i = 0; i < directories.Length; i++)
            {
                DirectoryInfo subDir = directories[i];
                results.Add(subDir);
                ScanDirectory(subDir, rank, ref results, ref currentRank);
            }
        }
    }
}
