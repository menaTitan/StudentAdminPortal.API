using AutoMapper;
using StudentAdminPortal.API.Profiles.AfterMap;

namespace StudentAdminPortal.API.Utility
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles() 
        {
            CreateMap<DataModels.Student, DomainModels.Student>().ReverseMap();
            CreateMap<DataModels.Address, DomainModels.Address>().ReverseMap();
            CreateMap<DataModels.Gender, DomainModels.Gender>().ReverseMap();
            CreateMap<DomainModels.UpdateStudentRequest, DataModels.Student>().AfterMap<UpdateStudentRequestAfterMap>();
            CreateMap<DomainModels.AddStudentRequest, DataModels.Student>().AfterMap<AddStudentRequestAfterMap>();
        }
    }
}
