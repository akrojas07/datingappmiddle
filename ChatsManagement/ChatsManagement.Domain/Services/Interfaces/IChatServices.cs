using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ChatsManagement.Domain.Models;

namespace ChatsManagement.Domain.Services.Interfaces
{
    public interface IChatServices
    {
        Task<List<DomainChat>> GetChatsByMatchId(long matchId);
        Task<List<DomainChat>> GetChatsByUserId(long userId);
        Task AddNewChatMessageByMatchId(DomainChat newChat);
        Task DeleteExistingChatMessage(long chatId);
    }
}
