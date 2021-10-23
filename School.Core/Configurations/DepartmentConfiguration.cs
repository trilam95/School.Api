using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using School.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Core.Configurations
{
    public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.ToTable("Department");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Budget).IsRequired();
            builder.Property(x => x.StartDate).IsRequired();
            builder.HasOne(x => x.Instructor).WithMany(x => x.Departments).HasForeignKey(x => x.InstructorId);
        }
    }
}
