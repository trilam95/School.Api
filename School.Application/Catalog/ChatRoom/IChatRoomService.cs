using System.Collections.Generic;
using System.Threading.Tasks;

namespace School.Application.Catalog.ChatRoom
{
    public interface IChatRoomService
    {
        Task<List<School.Core.Entities.ChatRoom>> GetChatRoomsAsync();
        Task<bool> AddChatRoomAsync(School.Core.Entities.ChatRoom newChatRoom);
    }
}
