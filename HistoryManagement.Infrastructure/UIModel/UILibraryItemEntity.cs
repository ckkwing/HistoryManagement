using IDAL.Model;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HistoryManagement.Infrastructure.UIModel
{
    public class UILibraryItemEntity : LibraryItemEntity
    {
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
