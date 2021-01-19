using System;
using System.Collections.Generic;
using System.Text;

using UMUser = ChatsManagement.Infrastructure.UserManagementAPI.Models.User;
using ChatsManagement.Domain.Models;

namespace ChatsManagement.Domain.Mapper
{
    public static class UserManagementToChatUserMapper
    {
        public static string UMUserToDomainChat(List<UMUser> umUsers, long userId)
        {
            string username = null; 

            foreach(var umUser in umUsers)
            {
                if (umUser.Id == userId)
                {
                    username = umUser.Username;
                    break;
                }
            }

            return username;
        }
    }
}
