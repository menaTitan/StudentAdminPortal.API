using AutoMapper;
using StudentAdminPortal.API.DomainModels;
using DataModel = StudentAdminPortal.API.DataModels;
namespace StudentAdminPortal.API.Profiles.AfterMap
{
    public class UpdateStudentRequestAfterMap : IMappingAction<UpdateStudentRequest, DataModel.Student>
    {
        public void Process(UpdateStudentRequest source, DataModels.Student destination, ResolutionContext context)
        {
            destination.Address = new DataModel.Address()
            {
                PhysicalAddress = source.PhysicalAddress,
                PostalAddress = source.PostalAddress,
            };
        }
    }
}
