﻿using IDAL.Model;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HistoryManagement.Infrastructure.UIModel
{
    public class UILibraryItemEntity : BindableBase
    {
        private int id = 0;
        private string path = string.Empty;
        private int level = 0;
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

        public static UILibraryItemEntity Create(LibraryItemEntity dbEntity)
        {
            return new UILibraryItemEntity()
            {
                ID = dbEntity.ID,
                Path = dbEntity.Path,
                Level = dbEntity.Level,
                IsDeleted = dbEntity.IsDeleted
            };
        }

        public static LibraryItemEntity ToDBEntity(UILibraryItemEntity uiEntity)
        {
            return new LibraryItemEntity()
            {
                ID = uiEntity.ID,
                Path = uiEntity.Path,
                Level = uiEntity.Level,
                IsDeleted = uiEntity.IsDeleted
            };
        }
    }
}
