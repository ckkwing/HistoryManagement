using IDAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    public interface IDBCategory
    {
        IList<CategoryEntity> GetItems();
        int InsertItems(IList<CategoryEntity> items);
        void DeleteItems(IList<CategoryEntity> items);
    }
}
