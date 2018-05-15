using IDAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Extension;

namespace HistoryManagement.Infrastructure.UIModel
{
    public class UIHistoryEntity : HistoryEntity
    {
        private bool isSelected = false;
        public bool IsSelected
        {
            get
            {
                return isSelected;
            }

            set
            {
                isSelected = value;
                RaisePropertyChanged("IsSelected");
            }
        }

        public static UIHistoryEntity Create(HistoryEntity historyEntity)
        {
            if (historyEntity.IsNull())
                throw new ArgumentNullException("Create UIHistoryEntity exception");

            UIHistoryEntity uiHistoryEntity = new UIHistoryEntity()
            {
                ID = historyEntity.ID,
                Path = historyEntity.Path,
                Name = historyEntity.Name,
                Star = historyEntity.Star,
                Comment = historyEntity.Comment,
                IsDeleted = historyEntity.IsDeleted,
                LibraryID = historyEntity.LibraryID
            };
            foreach (int cid in historyEntity.CategoryIDs)
            {
                uiHistoryEntity.CategoryIDs.Add(cid);
            }
            
            return uiHistoryEntity;
        }
    }
}
