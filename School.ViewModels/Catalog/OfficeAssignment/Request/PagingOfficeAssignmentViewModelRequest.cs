using School.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.ViewModels.Catalog.OfficeAssignment.Request
{
    public class PagingOfficeAssignmentViewModelRequest : PagingRequestBase
    {
        public string Location { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
