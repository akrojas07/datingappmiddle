using System;
using System.Collections.Generic;
using System.Text;
using ChatsManagement.Domain.Models;
using ChatsManagement.Infrastructure.Persistence.Entities;
using UMuser = ChatsManagement.Infrastructure.UserManagementAPI.Models.User;

namespace ChatsManagement.Domain.Mapper
{
    public static class EFChatsMapper
    {
        public static DomainChat DbToDomainChat(Chat dbChat)
        {
            DomainChat domainChat = new DomainChat()
            {
                Id = dbChat.Id,
                FirstUserId = dbChat.FirstUserId,
                SecondUserId = dbChat.SecondUserId,
                MatchId = dbChat.MatchId,
                Message = dbChat.Message,
                DateSent = (DateTime)dbChat.DateSent
            };

            return domainChat;
        }

        public static Chat DomainToDbChat(DomainChat domainChat)
        {
            Chat dbChat = new Chat()
            {
                Id = domainChat.Id,
                FirstUserId = domainChat.FirstUserId,
                SecondUserId = domainChat.SecondUserId,
                MatchId = domainChat.MatchId,
                Message = domainChat.Message,
                DateSent = domainChat.DateSent
            };

            return dbChat;
        }
        
    }
}
