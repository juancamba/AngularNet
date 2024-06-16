using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Api.Data;
using Api.DTOs;
using Api.Entities;
using Api.Extensions;
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
        private readonly IPhotoService _photoService;
        public UsersController(IUserRepository userRepository, IMapper mapper, IPhotoService photoService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _photoService = photoService;
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

        [HttpGet("{username}",Name ="GetUser")]
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
            //var username = User.GetUsername();
            //var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //var user = await _userRepository.GetUserByUsernameAsync(username);
                var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
            _mapper.Map(memberUpdateDto, user);
            _userRepository.Update(user);
            if(await _userRepository.SaveAllAsync())
                return NoContent();
            return BadRequest("Failed to update the user")    ;
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {

            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

            var result = await _photoService.AddPhotoAsync(file);
            
            if(result.Error !=null){
                return BadRequest(result.Error.Message);
            }

            var photo = new Photo{
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            
            };
            if(user.Photos.Count==0)
            {
                photo.IsMain = true;
            }
            user.Photos.Add(photo);
            if(await _userRepository.SaveAllAsync())
            {
                
                //llama al metdo GetUser pasandole el username que neceista el metodo ( el actual), luego el recurso
                return CreatedAtRoute("GetUser", new {Username = user.Username}, _mapper.Map<PhotoDto>(photo));
                
            }

            return BadRequest("Something went Wrong!");
        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            // el usuario autenticado
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

            var photo = user.Photos.FirstOrDefault(x=>x.Id == photoId);

            if(photo == null){
               return BadRequest("photo not exists!");
            }

            if(photo.IsMain){
               return BadRequest("this is already your main photo");
            }

            var currentMain = user.Photos.FirstOrDefault(x=>x.IsMain);
            if(currentMain!= null){
                currentMain.IsMain = false;
            }
            photo.IsMain = true;

            if(await _userRepository.SaveAllAsync())
                return NoContent();

            return BadRequest("Failed to set main photo");
        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if(photo == null){
                return NotFound();
            }

            if(photo.IsMain){
                return BadRequest("You cannot delete the main photo");
            }

            if(photo.PublicId != null){
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                if(result.Error !=null){
                    return BadRequest(result.Error.Message);
                }
            }

            user.Photos.Remove(photo);

            if(await _userRepository.SaveAllAsync()){
                return Ok();
            }

            return BadRequest("Failed to delete photo");
        }


    }
}