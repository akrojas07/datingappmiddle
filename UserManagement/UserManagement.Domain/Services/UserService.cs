﻿using System;
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
        public async Task<string> CreateNewUser(UserModel newUser)
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
                    var userName = await _userRepository.CreateNewUser(EfUserMapper.CoreModelToDbEntity(newUser));

                    //return user id
                    return userName;
                }

                // if user exists, throw exception
                throw new Exception("Username already exists");
            }
            else
            {
                //if input isn't valid, throw exception
                throw new Exception("Invalid User");
            }
        }

        /// <summary>
        /// Service method to delete individual user account
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>Completed task</returns>
        public async Task DeleteUserAccount(string userName)
        {
            //validate input
            if (userName == null)
            {
                throw new Exception("User not provided");
            }

            //pull user account
            var user = await _userRepository.GetUserByUserName(userName);

            //validate return
            if(user == null)
            {
                throw new Exception("User not found");
            }

            //delete user account
            await _userRepository.DeleteUserAccount(user.Id);
        }

        /// <summary>
        /// Service method to pull users from database by User Id
        /// </summary>
        /// <param name="userIds">List of User Ids</param>
        /// <returns>List of Domain Model Users</returns>
        public async Task<List<UserModel>> GetUsersByUserId(List<long> userIds)
        {
            //create empty list of domain users 
            List<UserModel> allUsers = new List<UserModel>();

            //pull all users from db
            var dbUsers = await _userRepository.GetUsersByUserId(userIds);

            if(dbUsers != null) 
            {
                //map each db user to Domain Model and add to domain users list
                foreach (var dbUser in dbUsers)
                {
                    allUsers.Add(EfUserMapper.DbEntityToCoreModel(dbUser));
                }
            }
            else
            {
                throw new Exception("No Users found");
            }


            //return domain users list
            return allUsers;
        }

        /// <summary>
        /// Service method to log a user in
        /// </summary>
        /// <param name="userName">Username</param>
        /// <param name="password">Password</param>
        /// <returns>username</returns>
        public async Task<string> Login(string userName, string password)
        {
            //validate inputs
            if(userName == null || password == null)
            {
                throw new Exception("Invalid input");
            }

            //pull user account
            var user = await _userRepository.GetUserByUserName(userName);

            //validate user exists
            if(user == null)
            {
                throw new Exception("Invalid user");
            }

            //if user exists validate passwords match
            if(user.Password != password)
            {
                throw new Exception("Invalid password");
            }

            //if passwords match, update user status 
            await _userRepository.UpdateUserStatus(user.Id, true);

            return userName;
        }

        /// <summary>
        /// Service method to log a user out
        /// </summary>
        /// <param name="userName">Username</param>
        /// <returns>Completed task</returns>
        public async Task Logout(string userName)
        {
            //validate inputs
            if (userName == null)
            {
                throw new Exception("Invalid input");
            }

            //pull user account
            var user = await _userRepository.GetUserByUserName(userName);

            //validate user exists
            if (user == null)
            {
                throw new Exception("Invalid user");
            }

            //if user is valid
            await _userRepository.UpdateUserStatus(user.Id, false);

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

            if (coreUser == null)
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
            else
            {
                result = true;
            };

            return result;

        }
    }
}
