using System;
using System.Collections.Generic;
using System.Text;

namespace School.Core.Entities
{
    public class Instructor : EntityBase
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime HireDate { get; set; }
        public List<Department> Departments { get; set; }
        public OfficeAssignment OfficeAssignment { get; set; }
        public List<Course> Courses { get; set; }
    }
}
