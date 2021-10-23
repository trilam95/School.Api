using School.ViewModels.Catalog.Course.Respone;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.ViewModels.Catalog.Department.Response
{
    public class GetGroupDepartmentViewModelResponse
    {
        public string DepartmentName { get; set; }
        public List<CourseViewModelResponse> Course { get; set; }
    }
}
