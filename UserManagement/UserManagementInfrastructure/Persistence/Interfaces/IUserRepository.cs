using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Infrastructure.Persistence.Entities;

namespace UserManagement.Infrastructure.Persistence.Interfaces
{
    public interface IUserRepository
    {
        Task<long> CreateNewUser(User newUser);
        Task<List<User>> GetAllUsers();
        Task<User> GetUserByUserName(string userName);
        Task UpdateUserProfile(User updatedUser);
        Task DeleteUserAccount(long userId);
    }
}
