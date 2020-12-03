using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UserManagementInfrastructure.Persistence.Interfaces;
using UserManagementInfrastructure.Repository.Entities;

namespace UserManagementInfrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        public Task<long> CreateNewUser(long userId)
        {
            throw new NotImplementedException();
        }

        public Task DeleteUserAccount(long userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<User>> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public Task UpdatePassword(long userId, string password)
        {
            throw new NotImplementedException();
        }

        public Task UpdateUserProfile(User updatedUser)
        {
            throw new NotImplementedException();
        }
    }
}
