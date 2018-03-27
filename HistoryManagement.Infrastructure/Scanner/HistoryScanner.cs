using HistoryManagement.Infrastructure.UIModel;
using IDAL.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Extension;

namespace HistoryManagement.Infrastructure.Scanner
{
    public class HistoryScanner
    {
        private IDictionary<LibraryItemEntity, IList<DirectoryInfo>> results = new Dictionary<LibraryItemEntity, IList<DirectoryInfo>>();
        public IDictionary<LibraryItemEntity, IList<DirectoryInfo>> Results
        {
            get
            {
                return results;
            }

            set
            {
                results = value;
            }
        }

        private IList<LibraryItemEntity> entityToScan = null;

        public HistoryScanner(IList<LibraryItemEntity> entityToScan)
        {
            this.entityToScan = entityToScan;
        }

        public void StartScan()
        {
            foreach(LibraryItemEntity entity in entityToScan)
            {
                if (!Directory.Exists(entity.Path))
                    break;
                DirectoryInfo dir = new DirectoryInfo(entity.Path);
                int currentRank = 0;
                int rank = entity.Level == -1 ? 10000000 : entity.Level;
                IList<DirectoryInfo> children = new List<DirectoryInfo>();
                ScanDirectory(dir, rank, ref children, ref currentRank);
                //categories.Add(dir.Name);
                Results.Add(new KeyValuePair<LibraryItemEntity, IList<DirectoryInfo>>(entity, children));
            }
        }

        private void ScanDirectory(DirectoryInfo directInfo, int rank, ref IList<DirectoryInfo> results, ref int currentRank)
        {
            if (directInfo.IsNull() || rank <= 0 || currentRank == rank)
                return;

            DirectoryInfo[] directories = directInfo.GetDirectories();
            currentRank++;
            if (directories.IsNull() || 0 == directories.Length)
            {
                results.Add(directInfo);
                return;
            }

            for (int i = 0; i < directories.Length; i++)
            {
                DirectoryInfo subDir = directories[i];
                //results.Add(subDir);
                ScanDirectory(subDir, rank, ref results, ref currentRank);
            }
        }
    }
}
