using School.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.ViewModels.Catalog.Instructor.Request
{
    public class PagingIntructorViewModelRequest : PagingRequestBase
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
    }
}
