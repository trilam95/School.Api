using System;

namespace School.Core.Entities
{
    public class Message : EntityBase
    {
        public Guid RoomId { get; set; }
        public string Contents { get; set; }
        public string UserName { get; set; }
        public DateTimeOffset PostedAt { get; set; }
    }
}
