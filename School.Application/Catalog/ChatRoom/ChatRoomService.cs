using Microsoft.EntityFrameworkCore;
using School.Core.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace School.Application.Catalog.ChatRoom
{
    public class ChatRoomService : IChatRoomService
    {
        private readonly IUnitOfWorks _unitOfWorks;

        public ChatRoomService(IUnitOfWorks unitOfWorks)
        {
            _unitOfWorks = unitOfWorks;
        }

        public async Task<List<School.Core.Entities.ChatRoom>> GetChatRoomsAsync()
        {
            var chatRooms = await _unitOfWorks.ChatRoomRepository.GetQuery().ToListAsync();

            return chatRooms;
        }

        public async Task<bool> AddChatRoomAsync(School.Core.Entities.ChatRoom chatRoom)
        {
            chatRoom.Id = Guid.NewGuid();

            _unitOfWorks.ChatRoomRepository.Create(chatRoom);

            await _unitOfWorks.SaveAsync();

            return true;
        }
    }
}
