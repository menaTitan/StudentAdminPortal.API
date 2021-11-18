using AutoMapper;

namespace StudentAdminPortal.API.Utility
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles() 
        {
            CreateMap<DataModels.Student, DomainModels.Student>();
        }
    }
}
