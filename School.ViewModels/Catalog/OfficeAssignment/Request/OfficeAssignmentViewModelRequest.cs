using System;
using System.Collections.Generic;
using System.Text;

namespace School.ViewModels.Catalog.OfficeAssignment.Request
{
    public class OfficeAssignmentViewModelRequest
    {
        public Guid Id { get; set; }
        public string Location { get; set; }
        public DateTime Timestamp { get; set; }
        public string InstructorLastName { get; set; }
        public OfficeAssignmentViewModelRequest()
        {
            this.Timestamp = DateTime.UtcNow;
        }
    }
}
