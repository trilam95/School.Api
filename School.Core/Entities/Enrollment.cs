using System;
using System.Collections.Generic;
using System.Text;

namespace School.Core.Entities
{
    public class Enrollment : EntityBase
    {
        public Guid CourseId { get; set; }
        public Course Course { get; set; }
        public Guid StudentId { get; set; }
        public Student Student { get; set; }
        public string Grade { get; set; }
    }
}
