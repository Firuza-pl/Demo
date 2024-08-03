using Demo.Domain.ViewModels.Courses;
using FluentValidation;

namespace ADO.API.Validator.Course
{
    public class CourseUpdateDTOValidator : AbstractValidator<CourseUpdateDTO>
    {
        public CourseUpdateDTOValidator() {
            RuleFor(x => x.CourseId).NotEmpty().WithMessage("id is required");
            RuleFor(x => x.CourseName).NotEmpty().WithMessage("cour name is required");
            RuleFor(x => x.Credits).NotEmpty().WithMessage("credits is required");

        }
    }
}
