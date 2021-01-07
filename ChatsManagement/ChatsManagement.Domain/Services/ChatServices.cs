using ChatsManagement.Domain.Models;
using ChatsManagement.Domain.Services.Interfaces;
using ChatsManagement.Infrastructure.Persistence.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ChatsManagement.Domain.Mapper;

namespace ChatsManagement.Domain.Services
{
    public class ChatServices : IChatServices
    {
        private readonly IChatRepository _chatRepository;

        public ChatServices(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }

        /// <summary>
        /// Service method to add new chat message by match id 
        /// </summary>
        /// <param name="newChat">Domain Chat object</param>
        /// <returns>Task Complete</returns>
        public async Task AddNewChatMessageByMatchId(DomainChat newChat)
        {
            if(newChat == null)
            {
                throw new ArgumentException();
            }

            var dbChat = EFChatsMapper.DomainToDbChat(newChat);

            await _chatRepository.AddNewChatMessageByMatchId(dbChat);
        }

        /// <summary>
        /// Service method to delete existing chat message by chat id
        /// </summary>
        /// <param name="chatId">Long Chat Id</param>
        /// <returns>Task Complete</returns>

        public async Task DeleteExistingChatMessage(long chatId)
        {
            if(chatId <= 0)
            {
                throw new ArgumentException();
            }

            await _chatRepository.DeleteExistingChatMessage(chatId);
        }


        /// <summary>
        /// Service Method to pull chats by match id
        /// </summary>
        /// <param name="matchId">Long Match Id</param>
        /// <returns>List of Domain Chats</returns>
        public async Task<List<DomainChat>> GetChatsByMatchId(long matchId)
        {
            //validate inputs 
            if(matchId <= 0)
            {
                throw new ArgumentException();
            }

            List<DomainChat> domainChats = new List<DomainChat>();

            //pull chats from db
            var dbChats = await _chatRepository.GetChatsByMatchId(matchId);

            //if chats exist, map db chats to domain chats 
            if(dbChats != null && dbChats.Count > 0)
            {
                foreach(var dbChat in dbChats)
                {
                    domainChats.Add(EFChatsMapper.DbToDomainChat(dbChat));
                }
            }

            //return list of domain chats
            return domainChats;

        }

        public async Task<List<DomainChat>> GetChatsByUserId(long userId)
        {
            if(userId <=0)
            {
                throw new ArgumentException();
            }
            List<DomainChat> domainChats = new List<DomainChat>();

            //pull chats from db
            var dbChats = await _chatRepository.GetChatsByUserId(userId);

            //if chats exist, map db chats to domain chats 
            if (dbChats != null && dbChats.Count > 0)
            {
                foreach (var dbChat in dbChats)
                {
                    domainChats.Add(EFChatsMapper.DbToDomainChat(dbChat));
                }
            }

            //return list of domain chats
            return domainChats;

        }
    }
}
