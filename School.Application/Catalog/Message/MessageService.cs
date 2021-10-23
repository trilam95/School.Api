using Microsoft.EntityFrameworkCore;
using School.Core.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace School.Application.Catalog.Message
{
    public class MessageService : IMessageService
    {
        private readonly IUnitOfWorks _unitOfWorks;

        public MessageService(IUnitOfWorks unitOfWorks)
        {
            _unitOfWorks = unitOfWorks;
        }

        public async Task<List<School.Core.Entities.Message>> GetMessagesAsync()
        {
            var messages = await _unitOfWorks.MessageRepository.GetQuery().ToListAsync();

            return messages;
        }

        public async Task<List<School.Core.Entities.Message>> GetMessagesForChatRoomAsync(Guid roomId)
        {
            var messagesForRoom = await _unitOfWorks.MessageRepository.GetQuery().Where(m => m.RoomId == roomId).ToListAsync();

            return messagesForRoom;
        }

        public async Task<bool> AddMessageToRoomAsync(Guid roomId, School.Core.Entities.Message message)
        {
            message.Id = Guid.NewGuid();
            message.RoomId = roomId;
            message.PostedAt = DateTimeOffset.Now;

            _unitOfWorks.MessageRepository.Create(message);

            await _unitOfWorks.SaveAsync();

            return true;
        }
    }
}
