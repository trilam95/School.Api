using System;
using System.Collections.Generic;
using System.Text;

namespace School.ViewModels.Catalog.Department.Request
{
    public class DepartmentViewModelRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Budget { get; set; }
        public DateTime StartDate { get; set; }
        public string InstructorLastName { get; set; }
        public DepartmentViewModelRequest()
        {
            this.StartDate = DateTime.UtcNow;
        }
    }
}
