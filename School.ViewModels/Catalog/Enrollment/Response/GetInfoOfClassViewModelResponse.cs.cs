using System;
using System.Collections.Generic;
using System.Text;

namespace School.ViewModels.Catalog.Enrollment.Response
{
    public class GetInfoOfClassViewModelResponse
    {
        public string Grade { get; set; }
        public List<StudentAndCourseInfo> InfoEnrollment { get; set; }
    }

    public class StudentAndCourseInfo
    {
        public string FullName { get; set; }
        public string Title { get; set; }
    }
}
