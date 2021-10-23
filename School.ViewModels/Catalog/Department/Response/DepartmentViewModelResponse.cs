using System;
using System.Collections.Generic;
using System.Text;

namespace School.ViewModels.Catalog.Department.Response
{
    public class DepartmentViewModelResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Budget { get; set; }
        public DateTime StartDate { get; set; }
    }
}
