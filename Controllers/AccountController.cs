using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Api.Data;
using Api.DTOs;
using Api.Entities;
using Api.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        private readonly IMapper _maper;

        public AccountController(DataContext context, ITokenService tokenService, IMapper maper)
        {
            _context = context;
            _tokenService = tokenService;
            _maper = maper;
        }



        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if(await UserExists(registerDto.Username)) 
                return BadRequest("Username already taken");

            var user = _maper.Map<AppUser>(registerDto);
            
            using var hmac = new HMACSHA512();

            user.Username = registerDto.Username.ToLower();
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
            user.PasswordSalt = hmac.Key;
            

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserDto{
                Username = user.Username,
                Token = _tokenService.CreateToken(user),
                //PhotoUrl = user.Photos.FirstOrDefault(x=>x.IsMain)?.Url,
                KnownAs = user.KnownAs,
                Gender = user.Gender
            };
        }

        public async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(x=> x.Username == username.ToLower());
        }

        
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _context.Users
                .Include(p=>p.Photos)
                .SingleOrDefaultAsync(x => x.Username == loginDto.Username);
            
            if(user == null)
                return Unauthorized("Invalid username");

            using var hmac = new  HMACSHA512(user.PasswordSalt);

            var ComputedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for(int i = 0; i < ComputedHash.Length; i++)
            {
                if(ComputedHash[i]!=user.PasswordHash[i])
                    return Unauthorized("Invalid password");
            }

            return new UserDto{
                Username = user.Username,
                Token = _tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(x=>x.IsMain)?.Url,
                KnownAs = user.KnownAs,
                Gender = user.Gender

            };
        }
    }
}