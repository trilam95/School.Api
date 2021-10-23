using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using School.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Core.Configurations
{
    public class StudentImageConfiguration : IEntityTypeConfiguration<StudentImage>
    {
        public void Configure(EntityTypeBuilder<StudentImage> builder)
        {
            builder.ToTable("StudentImage");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.ImagePath).HasMaxLength(200).IsRequired(true);
            builder.Property(x => x.Caption).HasMaxLength(200);
            builder.HasOne(x => x.Student).WithMany(x => x.StudentImages).HasForeignKey(x => x.StudentId);
        }
    }
}
