using System;
using System.Collections.Generic;
using System.Text;

namespace School.Core.Entities
{
    public class Student : EntityBase
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public List<Enrollment> Enrollments { get; set; }
        public List<StudentImage> StudentImages { get; set; }
    }
}
