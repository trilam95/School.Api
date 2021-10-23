using System;

namespace School.ViewModels.Catalog.Course.Request
{
    public class CourseViewModelRequest
    {
        public Guid? Id { get; set; }
        public string Title { get; set; }
        public int Credits { get; set; }
        public string DepartmentName { get; set; }
    }
}
