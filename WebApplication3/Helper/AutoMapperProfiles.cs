using AutoMapper;
using SampleAPI.DTOs;
using SampleAPI.Helpers;
using SampleAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleAPI.Helper
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserForListDTO>().
                ForMember(dest => dest.PhotoUrl, 
                         opt => {
                             opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.Ismain).Url); 
                                 })
                                 .ForMember(dest=>dest.Age,opt=> {
                                     opt.ResolveUsing(d => d.DateOfBirth.CalculateAge());
                                 });
            CreateMap<User, UserForDetailedDTO>().
                      ForMember(dest => dest.PhotoUrl,
                         opt => {
                             opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.Ismain).Url);
                         })
                         .ForMember(dest => dest.Age, opt => {
                             opt.ResolveUsing(d => d.DateOfBirth.CalculateAge());
                         });
            CreateMap<Photo, PhotosForDetailedDTO>();
            CreateMap<UserForUpdateDTO, User>();
        }
    }
}
