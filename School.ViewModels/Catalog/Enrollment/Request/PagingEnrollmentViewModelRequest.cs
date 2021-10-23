using School.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.ViewModels.Catalog.Enrollment.Request
{
    public class PagingEnrollmentViewModelRequest : PagingRequestBase
    {
        public string Grade { get; set; }
    }
}
