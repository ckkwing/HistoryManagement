using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL.Model
{
    public class HistoryEntity : BindableBase
    {
        public int ID { get; set; }
        public string Path { get; set; }
        public string Name { get; set; }
        public int Star { get; set; }
        public string Comment { get; set; }
        public IList<int> CategoryIDs { get; set; } = new List<int>();
        public bool IsDeleted { get; set; }
        public int LibraryID { get; set; }
    }
}
