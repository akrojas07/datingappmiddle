using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using UserManagement.Domain.Services.Interfaces;
using UserManagement.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using UserManagement.Domain.Models;

namespace UserManagement.API.Controllers
{
    [Route("/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _config;
        private string _tokenstring;

        public UserController(IUserService userService, IConfiguration config)
        {
            _userService = userService;
            _config = config;
        }


        [HttpPost]
        [Route("new")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateNewUser([FromBody] NewUserRequest newUser)
        {
            //validate entry
            var result = ValidateEntry(newUser);
            if (!result)
            {
                return StatusCode(400, "Invalid user entry");
            }

            try
            {
                //convert from request to user model 
                var modelUser = new UserModel()
                {
                    FirstName = newUser.FirstName,
                    LastName = newUser.LastName,
                    Username = newUser.Username,
                    Password = newUser.Password
                };

                //pass model user into service and generate JSON Token 
                var username = await _userService.CreateNewUser(modelUser);
                _tokenstring = GenerateJsonWebToken();

                return StatusCode(201, new { un = username, token = _tokenstring });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpDelete]
        [Route("delete/{username}")]
        public async Task<IActionResult> DeleteUser(string username, string token)
        {
            //validate username is not empty
            if (username == null)
            {
                return StatusCode(400, "User not provided");
            }

            //validate token
            if (_tokenstring != token)
            {
                //if token doesn't match, return unauthorized 
                return StatusCode(401);
            }

            try
            {
                //call delete user account service
                await _userService.DeleteUserAccount(username);

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        private bool ValidateEntry(BaseUserRequest baseRequest)
        {
            if (baseRequest == null)
            {
                return false;
            }
            else if (baseRequest.FirstName == null)
            {
                return false;
            }
            else if (baseRequest.LastName == null)
            {
                return false;
            }
            else if (baseRequest.Password == null)
            {
                return false;
            }
            else if (baseRequest.Username == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        private string GenerateJsonWebToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Jwt:Key").Value));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config.GetSection("Jwt:Issuer").Value,
                    _config.GetSection("Jwt:Issuer").Value,
                    null,
                    expires: DateTime.Now.AddMinutes(120),
                    signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
