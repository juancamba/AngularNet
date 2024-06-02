using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Data;
using Api.Entities;
using Api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    
    [Authorize]
    
    public class UsersController : BaseApiController
    {
        
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            return Ok(await  _userRepository.GetUsersAsync());
        }

        [HttpGet("{username}")]
        //[Authorize]
        public async Task<ActionResult<AppUser>> GetUser(string username)
        {
            var users = await _userRepository.GetUserByUsernameAsync(username);
            return users;
        }
    }
}