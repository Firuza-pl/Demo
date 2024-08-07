using Demo.Domain.ViewModels.Students;
using FluentValidation;
namespace ADO.API.Validator;
public class StudentUpdateDTOValidator : AbstractValidator<StudentUpdateDTO>
{
    public StudentUpdateDTOValidator()
    {
        RuleFor(x => x.StudentId).NotEmpty().WithMessage("Id is required.");
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name is required.");
        RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name is required.");
        RuleFor(x => x.Email).EmailAddress().WithMessage("Provide valid Email address");
    }

}

