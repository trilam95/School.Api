using System;
using System.Collections.Generic;
using System.Text;

namespace School.ViewModels.Catalog.Instructor.Request
{
    public class InstructorViewModelRequest
    {
        public Guid Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime HireDate { get; set; }
        public InstructorViewModelRequest()
        {
            this.HireDate = DateTime.UtcNow;
        }
    }
}
