﻿using HistoryManagement.Infrastructure.UIModel;
using IDAL.Model;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HistoryManagement.Infrastructure.Events
{
    public enum SelectionType
    {
        Category,
        Library,
    }
    public class ContentSelectionEventArgs : EventArgs
    {
        public SelectionType SelectionType { get; set; }
        public UICategoryEntity UICategoryEntity { get; set; }
        public LibraryItemEntity LibraryItemEntity { get; set; }
    }

    public class ContentSelectionEvents : PubSubEvent<ContentSelectionEventArgs>
    {

    }
}
