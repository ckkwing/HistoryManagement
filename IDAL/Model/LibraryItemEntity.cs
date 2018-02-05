using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL.Model
{
    public class LibraryItemEntity
    {
        public int ID { get; set; }
        public string Path { get; set; }
        public int Level { get; set; }
        public bool IsDeleted { get; set; }
    }
}
