using System;
using System.Collections.Generic;
using System.Text;

namespace School.Core.Entities
{
    public class Department : EntityBase
    {
        public string Name { get; set; }
        public double Budget { get; set; }
        public DateTime StartDate { get; set; } 
        public Guid InstructorId { get; set; }
        public Instructor Instructor { get; set; }
        public List<Course> Courses { get; set; }
    }
}
