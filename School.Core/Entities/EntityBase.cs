using System;
using System.Collections.Generic;
using System.Text;

namespace School.Core.Entities
{
    public class EntityBase
    {
        public Guid Id { get; set; }

        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        public bool isDelete { get; set; }


        public EntityBase()
        {
            this.CreatedDate = DateTime.UtcNow;
            this.UpdatedDate = DateTime.UtcNow;
        }

    }
}
