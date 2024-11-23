using AutoMapper;
using _301247589_301276375_bright_aid_API.Models;
using _301247589_301276375_bright_aid_API.DTOs;

namespace _301247589_301276375_bright_aid_API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Student, StudentDto>(); // Map Student entity to StudentDto
            CreateMap<StudentForCreationDto, Student>(); // Map StudentForCreationDto to Student
            CreateMap<StudentForUpdateDto, Student>(); // Map StudentForUpdateDto to Student

            CreateMap<Donor, DonorDto>(); // Map Donor entity to DonorDto
            CreateMap<DonorForCreationDto, Donor>(); // Map DonorForCreationDto to Donor
            CreateMap<DonorForUpdateDto, Donor>(); // Map DonorForUpdateDto to Donor
        }
    }
}
