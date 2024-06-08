using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Api.Data;
using Api.DTOs;
using Api.Entities;
using Api.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    
    [Authorize]
    
    public class UsersController : BaseApiController
    {
        
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;

        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            /*
            var users = await  _userRepository.GetUsersAsync();
            var usersToReturn = _mapper.Map<IEnumerable<MemberDto>>(users);
            return Ok(usersToReturn);
            */
            var users = await _userRepository.GetMembersAsync();
            return Ok(users);
        }

        [HttpGet("{username}")]
        //[Authorize]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
          /*  
            a la vieja escuela, 
            var user = await _userRepository.GetUserByUsernameAsync(username);
            return _mapper.Map<MemberDto>(user);
            */
            //ya recupero desde el repository , el dtoddd
            return await _userRepository.GetMemberAsync(username);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            
            // recupera el usernmae del token
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userRepository.GetUserByUsernameAsync(username);

            _mapper.Map(memberUpdateDto, user);
            _userRepository.Update(user);
            if(await _userRepository.SaveAllAsync())
                return NoContent();
            return BadRequest("Failed to update the user")    ;
        }
    }
}