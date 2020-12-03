using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Domain.Models;
using UserManagement.Domain.Services.Interfaces;
using UserManagement.Domain.Mapper;
using UserManagement.Infrastructure.Persistence.Interfaces;

namespace UserManagement.Domain.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository) 
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Service method to Create new Core User
        /// </summary>
        /// <param name="newUser">User Model</param>
        /// <returns>User Id </returns>
        public async Task<long> CreateNewUser(UserModel newUser)
        {
            //validate new user 
            bool validUser = ValidateUser(newUser);

            if (validUser)
            {
                //if user properties are valid
                try
                {
                    //check to see if user by user name exists 
                    var existingUser = await _userRepository.GetUserByUserName(newUser.Username);
                    if(existingUser == null)
                    {
                        throw new Exception("User does not exist");
                    }
                }
                catch
                {
                    //if user doesn't exist, create new user and store user id in userId
                    var userId = await _userRepository.CreateNewUser(EfUserMapper.CoreModelToDbEntity(newUser));

                    //return user id
                    return userId;
                }

                // if user exists, throw exception
                throw new Exception("Username already exists");
            }
            else
            {
                throw new Exception("Invalid User");
            }
        }

        /// <summary>
        /// Service method to delete individual user account
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>Completed task</returns>
        public async Task DeleteUserAccount(long userId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Service method to pull all users from database
        /// </summary>
        /// <returns>List of Domain Model Users</returns>
        public async Task<List<UserModel>> GetAllUsers()
        {
            //create empty list of domain users 
            List<UserModel> allUsers = new List<UserModel>();

            //pull all users from db
            var dbUsers = await _userRepository.GetAllUsers();

            //map each db user to Domain Model and add to domain users list
            foreach(var dbUser in dbUsers)
            {
                allUsers.Add(EfUserMapper.DbEntityToCoreModel(dbUser));
            }

            //return domain users list
            return allUsers;
        }
        /// <summary>
        /// Service method to update individual user profile information
        /// </summary>
        /// <param name="updatedUser">User Model</param>
        /// <returns>Completed Task</returns>

        public async Task UpdateUserProfile(UserModel updatedUser)
        {
            //validate updatedUser
            var validUser = ValidateUser(updatedUser);

            //if updatedUser is valid
            if (validUser)
            {
                //pull user account
                var user = await _userRepository.GetUserByUserName(updatedUser.Username);

                //validate user
                if(user == null)
                {
                    throw new Exception("Invalid User");
                }

                //map updatedUser to user
                user = EfUserMapper.CoreModelToDbEntity(updatedUser);

                //submit updated information to repository
                await _userRepository.UpdateUserProfile(user);
                
            }
            else
            {
                throw new Exception("Invalid User");
            }

        }

        /// <summary>
        /// Private method to validate user properties
        /// </summary>
        /// <param name="coreUser">User Model</param>
        /// <returns>True if user is valid</returns>
        private bool ValidateUser(UserModel coreUser)
        {
            bool result;

            if(coreUser == null)
            {
                result = false;
            }
            else if(coreUser.FirstName == null)
            {
                result = false;
            }
            else if(coreUser.LastName == null)
            {
                result = false;
            }
            else if(coreUser.Password == null)
            {
                result = false;
            }
            else if(coreUser.Username == null)
            {
                result = false;
            }
            else if(coreUser.Id == 0)
            {
                result = false;
            }
            else
            {
                result = true;
            };

            return result;

        }
    }
}
