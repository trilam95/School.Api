using System;
using System.Collections.Generic;
using System.Text;

namespace School.Core.Entities
{
    public class OfficeAssignment : EntityBase
    {
        public string Location { get; set; }
        public DateTime Timestamp { get; set; }
        public Guid InstructorId { get; set; }
        public Instructor Instructor { get; set; }
    }
}
