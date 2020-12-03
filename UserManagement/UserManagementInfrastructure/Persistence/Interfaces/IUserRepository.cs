using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UserManagementInfrastructure.Repository.Entities;

namespace UserManagementInfrastructure.Persistence.Interfaces
{
    interface IUserRepository
    {
        Task<long> CreateNewUser(long userId);
        Task<List<User>> GetAllUsers();
        Task UpdatePassword(long userId, string password);
        Task UpdateUserProfile(User updatedUser);
        Task DeleteUserAccount(long userId);
    }
}
