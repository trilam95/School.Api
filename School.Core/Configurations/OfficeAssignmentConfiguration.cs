using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using School.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Core.Configurations
{
    public class OfficeAssignmentConfiguration : IEntityTypeConfiguration<OfficeAssignment>
    {
        public void Configure(EntityTypeBuilder<OfficeAssignment> builder)
        {
            builder.ToTable("OfficeAssignment");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Location).IsRequired();
            builder.Property(x => x.Timestamp).IsRequired();
        }
    }
}
