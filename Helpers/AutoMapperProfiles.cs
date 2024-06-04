using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.DTOs;
using Api.Entities;
using Api.Extensions;
using AutoMapper;

namespace Api.Helpers
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, MemberDto>()
                .ForMember(dest=>dest.PhotoUrl, opt=>opt.MapFrom(src=>src.Photos.FirstOrDefault(x=>x.IsMain).Url))
                .ForMember(dest=>dest.Age, opt=>opt.MapFrom(s=>s.DateOfBirth.CalculateAge()))
                ;
            CreateMap<Photo, PhotoDto>();
        }
    }
}