using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using School.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Core.Configurations
{
    public class InstructorConfiguration : IEntityTypeConfiguration<Instructor>
    {
        public void Configure(EntityTypeBuilder<Instructor> builder)
        {
            builder.ToTable("Instructor");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.LastName).IsRequired();
            builder.Property(x => x.FirstName).IsRequired();
            builder.Property(x => x.HireDate).IsRequired();
            builder.HasMany(x => x.Courses);
        }
    }
}
