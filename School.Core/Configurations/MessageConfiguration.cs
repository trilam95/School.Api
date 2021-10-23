using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using School.Core.Entities;

namespace School.Core.Configurations
{
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.ToTable("Message");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.RoomId).IsRequired();
            builder.Property(x => x.Contents).IsRequired();
            builder.Property(x => x.UserName).IsRequired();
            builder.Property(x => x.PostedAt).IsRequired();
        }
    }
}
