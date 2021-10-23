using System;
using System.Collections.Generic;
using System.Text;

namespace School.Core.Entities
{
    public class StudentImage :EntityBase
    {
        public Guid StudentId { get; set; }
        public string ImagePath { get; set; }
        public string Caption { get; set; }
        public int SortOrder { get; set; }
        public long FileSize { get; set; }

        public Student Student { get; set; }
    }
}
