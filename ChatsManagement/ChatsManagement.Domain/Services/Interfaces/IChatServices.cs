using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ChatsManagement.Domain.Models;

namespace ChatsManagement.Domain.Services.Interfaces
{
    public interface IChatServices
    {
        Task<List<DomainChat>> GetChatsByMatchId(long matchId, string token);
        Task<List<DomainChat>> GetChatsByUserId(long userId, string token);
        Task<long> AddNewChatMessageByMatchId(DomainChat newChat, string token);
        Task DeleteExistingChatMessage(long chatId);
    }
}
