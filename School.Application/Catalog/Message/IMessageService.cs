using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace School.Application.Catalog.Message
{
    public interface IMessageService
    {
        Task<List<School.Core.Entities.Message>> GetMessagesAsync();
        Task<List<School.Core.Entities.Message>> GetMessagesForChatRoomAsync(Guid roomId);
        Task<bool> AddMessageToRoomAsync(Guid roomId, School.Core.Entities.Message message);
    }
}
