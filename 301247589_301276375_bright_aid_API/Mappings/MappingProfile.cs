using AutoMapper;
using _301247589_301276375_bright_aid_API.Models;
using _301247589_301276375_bright_aid_API.DTOs;

namespace _301247589_301276375_bright_aid_API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Map Student entity to StudentDto
            CreateMap<Student, StudentDto>()
           .ForMember(dest => dest.FullName,
                      opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));


            CreateMap<StudentForCreationDto, Student>(); // Map StudentForCreationDto to Student
            CreateMap<StudentForUpdateDto, Student>(); // Map StudentForUpdateDto to Student

            // Map Donor entity to DonorDto
            // Map Student entity to StudentDto
            CreateMap<Donor, DonorDto>()
           .ForMember(dest => dest.FullName,
                      opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));

            CreateMap<DonorForCreationDto, Donor>(); // Map DonorForCreationDto to Donor
            CreateMap<DonorForUpdateDto, Donor>(); // Map DonorForUpdateDto to Donor
        }
    }
}
