using FluentValidation;
using StudentAdminPortal.API.DomainModels;
using StudentAdminPortal.API.Repositories;

namespace StudentAdminPortal.API.Validators;

public class UpdateStudentRequestValidator : AbstractValidator<UpdateStudentRequest>
{
    public UpdateStudentRequestValidator(IStudentRepository studentRepository)
    {
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
        RuleFor(x => x.DateOfBirth).NotEmpty();
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Mobile).GreaterThan(9999999).LessThan(9999999999);
        RuleFor(x => x.GenderId).NotEmpty().Must(id =>
        {
            var gender = studentRepository.GetGendersAsync().Result.FirstOrDefault(g => g.Id == id);
            return gender != null;

        }).WithMessage("Please select a valid Gender");

        RuleFor(x => x.PhysicalAddress).NotEmpty();
        RuleFor(x => x.PostalAddress).NotEmpty();
    }

}