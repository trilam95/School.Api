using System;
using System.Collections.Generic;
using System.Text;

namespace School.ViewModels.Catalog.Instructor.Response
{
    public class InstructorViewModelResponse
    {
        public Guid Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime HireDate { get; set; }
    }
}
