using System;
using System.Collections.Generic;
using System.Text;

namespace School.ViewModels.Catalog.Enrollment.Request
{
    public class EnrollmentViewModelRequest
    {
        public Guid Id { get; set; }
        public string Grade { get; set; }
        public string CourseName { get; set; }
        public string StudentLastName { get; set; }
    }
}
