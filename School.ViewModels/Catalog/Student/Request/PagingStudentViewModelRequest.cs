using School.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.ViewModels.Catalog.Student.Request
{
    public class PagingStudentViewModelRequest : PagingRequestBase
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime EnrollmentDate { get; set; }
    }
}
