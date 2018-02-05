using IDAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    public interface IDBHistory
    {
        IList<HistoryEntity> GetItems();
        int InsertItems(IList<HistoryEntity> items);
        void DeleteItems(IList<HistoryEntity> items);
    }
}
