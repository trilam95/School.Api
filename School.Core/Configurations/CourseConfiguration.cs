using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using School.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Core.Configurations
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.ToTable("Course");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Title).IsRequired();
            builder.Property(x => x.Credits).IsRequired();

            builder.HasOne(x => x.Department).WithMany(x => x.Courses).HasForeignKey(x => x.DepartmentId);
            builder.HasMany(x => x.Instructors);
        }
    }
}
