using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ChatsManagement.Infrastructure.Persistence.Entities;

namespace ChatsManagement.Infrastructure.Persistence.Repository.Interfaces
{
    public interface IChatRepository
    {
        Task <List<Chat>> GetChatsByMatchId(long matchId);

        Task<List<Chat>> GetChatsByUserId(long userId);
        Task AddNewChatMessageByMatchId(Chat newChat);
        Task DeleteExistingChatMessage(long chatId);

    }
}
