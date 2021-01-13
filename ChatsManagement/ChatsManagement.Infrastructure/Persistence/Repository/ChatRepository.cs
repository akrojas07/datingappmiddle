using ChatsManagement.Infrastructure.Persistence.Entities;
using ChatsManagement.Infrastructure.Persistence.Repository.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ChatsManagement.Infrastructure.Persistence.Repository
{
    public class ChatRepository : IChatRepository
    {
        /// <summary>
        /// Repository method to add new chat to chat table based on match id
        /// </summary>
        /// <param name="newChat">Chat Entity</param>
        /// <returns>Completed Task</returns>
        public async Task<long> AddNewChatMessageByMatchId(Chat newChat)
        {
            using (var context = new DatingAppContext())
            {
                context.Chats.Add(newChat);
                await context.SaveChangesAsync();

                var chat = await context.Chats.Where(c => c.FirstUserId == newChat.FirstUserId && c.SecondUserId == newChat.SecondUserId)
                    .OrderBy(c => c.Id)
                    .LastOrDefaultAsync();

                return chat.Id;
            }

        }

        /// <summary>
        /// Repository method to delete existing chat message by chat id
        /// </summary>
        /// <param name="chatId">Long</param>
        /// <returns>Task complete</returns>
        public async Task DeleteExistingChatMessage(long chatId)
        {
            using (var context = new DatingAppContext())
            {
                //pull existing chat 
                var chat = await context.Chats.FirstOrDefaultAsync(c => c.Id == chatId);

                //remove
                context.Chats.Remove(chat);
                await context.SaveChangesAsync();

            }
        }

        /// <summary>
        /// Repository Method to pull chats by match id
        /// </summary>
        /// <param name="matchId">Long</param>
        /// <returns>List of Chat Objects</returns>
        public async Task<List<Chat>> GetChatsByMatchId(long matchId)
        {
            using (var context = new DatingAppContext())
            {
                return await context.Chats.Where(c => c.MatchId == matchId).ToListAsync();
            }
        }

        /// <summary>
        /// Repository method to pull all chats by user id
        /// </summary>
        /// <param name="userId">Long user id</param>
        /// <returns>List of Chat objects </returns>
        public async Task<List<Chat>> GetChatsByUserId(long userId)
        {
            using (var context = new DatingAppContext())
            {
                return await context.Chats.Where(c => c.FirstUserId == userId || c.SecondUserId == userId).ToListAsync();
            }
        }
    }
}
