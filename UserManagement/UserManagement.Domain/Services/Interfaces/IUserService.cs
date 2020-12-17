using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Domain.Models;

namespace UserManagement.Domain.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserModel> CreateNewUser(UserModel newUser);
        Task<UserModel> GetUserByUsername(string username);
        Task<List<UserModel>> GetUsersByUserId(List<long> userIds);
        Task UpdateUserProfile(UserModel updatedUser);
        Task DeleteUserAccount(string userName);
        Task<UserModel> Login(string userName, string password);
        Task Logout(string userName);
    }
}
