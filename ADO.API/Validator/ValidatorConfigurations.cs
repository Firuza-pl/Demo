using ADO.API.Validator.Course;
using FluentValidation;
namespace ADO.API.Validator;
public static class ValidatorConfigurations
{
    public  static void AddValidators( this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<StudentCreatedDTOValidator>();
        services.AddValidatorsFromAssemblyContaining<StudentUpdateDTOValidator>();

        services.AddValidatorsFromAssemblyContaining<CourseUpdateDTOValidator>();
        services.AddValidatorsFromAssemblyContaining<CourseCreatedDTOValidator>();
    }
}
