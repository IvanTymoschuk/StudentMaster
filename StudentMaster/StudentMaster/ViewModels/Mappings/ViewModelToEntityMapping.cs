using AutoMapper;
using StudentMaster.Models;
using StudentMaster.ViewModels.AdminViewModels;
using System;

namespace BackEnd.ViewModels.Mappings
{
    public class ViewModelToEntityMappingProfile : Profile
    {
        public ViewModelToEntityMappingProfile()
        {
 

            CreateMap<User, UserViewModel>()
               .ForMember(dest => dest.Age,
                   opts => opts.MapFrom(
                       src =>DateTime.Now.Year-src.BirthDate.Year
                   ))
                   .ForMember(dest => dest.Email,
                   opts => opts.MapFrom(
                       src => src.Email
                   ))
                   .ForMember(dest => dest.FirstName,
                   opts => opts.MapFrom(
                       src => src.FirstName
                   ))
                   .ForMember(dest => dest.LastName,
                   opts => opts.MapFrom(
                       src => src.LastName
                   ))
                   .ForMember(dest => dest.RegistredDate,
                   opts => opts.MapFrom(
                       src => src.RegistrationDate
                   ))
                   .ForMember(dest => dest.StudyDate,
                   opts => opts.MapFrom(
                       src => src.StudyDate
                   ))
                   .ForMember(dest => dest.UserId,
                   opts => opts.MapFrom(
                       src => src.Id
                   ))
                   .ReverseMap();

        }
    }
}