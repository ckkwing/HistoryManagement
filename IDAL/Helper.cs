using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace IDAL
{
    class Helper
    {
        private static string databaseFolder;
        public static string DatabaseFolder
        {
            get
            {
                if (string.IsNullOrEmpty(databaseFolder))
                {
                    databaseFolder = Path.Combine(FolderDefine.Instance.UserDataFolder, "Database");
                    if (!Directory.Exists(databaseFolder))
                    {
                        Directory.CreateDirectory(databaseFolder);
                    }
                }
                return databaseFolder;
            }
        }
    }
}
