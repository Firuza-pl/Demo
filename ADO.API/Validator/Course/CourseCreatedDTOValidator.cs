using Demo.Domain.ViewModels.Courses;
using FluentValidation;

namespace ADO.API.Validator.Course;
public class CourseCreatedDTOValidator : AbstractValidator<CourseCreatedDTO>
{
    public CourseCreatedDTOValidator()
    {

        RuleFor(x => x.CourseName).NotEmpty().WithMessage("cour name is required");
        RuleFor(x => x.Credits).NotEmpty().WithMessage("credits is required");
    }
}
