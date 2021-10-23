using School.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.ViewModels.Catalog.Department.Request
{
    public class PagingDepartmentViewModelRequest : PagingRequestBase
    {
        public string Name { get; set; }
        public double Budget { get; set; }
        //public DateTime StartDate { get; set; }
    }
}
