using IDAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    public interface IDBLibrary
    {
        IList<LibraryItemEntity> GetItems();
        int InsertItems(IList<LibraryItemEntity> items);
        void DeleteItems(IList<LibraryItemEntity> items);
    }
}
