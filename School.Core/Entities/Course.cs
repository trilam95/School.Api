using System;
using System.Collections.Generic;

namespace School.Core.Entities
{
    public class Course : EntityBase
    {
        public string Title { get; set; }
        public int Credits { get; set; }
        public Guid DepartmentId { get; set; }
        public Department Department { get; set; }
        public List<Enrollment> Enrollments { get; set; }
        public List<Instructor> Instructors { get; set; }
    }
}
