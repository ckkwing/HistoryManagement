using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HistoryManagement.Infrastructure.Events
{
    public enum DBDataType
    {
        All,
        Category ,
        Library,
        History
    }

    public class DBDataRefreshedEventEventArgs : EventArgs
    {
        public DBDataType DBDataType { get; set; }
    }

    public class DBDataRefreshedEvents : PubSubEvent<DBDataRefreshedEventEventArgs>
    {

    }
}
