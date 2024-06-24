using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.DTOs;
using Api.Entities;
using Api.Helpers;
using Api.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace Api.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public UserRepository(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<MemberDto> GetMemberAsync(string username)
        {
            return await _dataContext.Users
              .Where(x => x.Username == username)
              .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();
        }

        public async Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParms)
        {
            var query =  _dataContext.Users.AsQueryable() ;

            query = query.Where(u=>u.Username != userParms.CurrentUsername);
            query = query.Where(u=>u.Gender != userParms.Gender);

            var minDob = DateTime.Today.AddYears(-userParms.MaxAge -1);
            var maxDob = DateTime.Today.AddYears(-userParms.MinAge);

            query = query.Where(u=>u.DateOfBirth >= minDob && u.DateOfBirth <=maxDob);

            return await PagedList<MemberDto>.CreateAsync(
                query.ProjectTo<MemberDto>(_mapper.ConfigurationProvider).AsNoTracking()
                , userParms.PageNumber,userParms.PageSize);
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _dataContext.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await _dataContext.Users
                .Include(p=>p.Photos)
                .SingleOrDefaultAsync(x=>x.Username == username);
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await _dataContext.Users
            .Include(p=>p.Photos)
            .ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _dataContext.SaveChangesAsync()>0;
        }

        public void Update(AppUser user)
        {
            // marcamos para modificarlo
            _dataContext.Entry(user).State = EntityState.Modified;
        }
    }
}