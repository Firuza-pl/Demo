using Demo.Domain.ViewModels.Students;
using FluentValidation;

namespace ADO.API.Validator;
public class StudentCreatedDTOValidator : AbstractValidator<StudentCreatedDTO>
{
    public StudentCreatedDTOValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name is required.");
        RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name is required.");
        // Add additional validation rules as needed
    }
}
