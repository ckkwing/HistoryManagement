﻿using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL.Model
{
    public class LibraryItemEntity : BindableBase
    {
        private int id = 0;
        private string path = string.Empty;
        private int level = -1; //-1 is unlimited
        private bool isDeleted = false;

        public int ID
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
                RaisePropertyChanged("ID");
            }
        }

        public string Name
        {
            get
            {
                return System.IO.Path.GetFileName(path);
            }
        }

        public string Path
        {
            get
            {
                return path;
            }

            set
            {
                path = value;
                RaisePropertyChanged("Path");
            }
        }

        public int Level
        {
            get
            {
                return level;
            }

            set
            {
                level = value;
                RaisePropertyChanged("Level");
            }
        }

        public bool IsDeleted
        {
            get
            {
                return isDeleted;
            }

            set
            {
                isDeleted = value;
                RaisePropertyChanged("IsDeleted");
            }
        }
    }
}
