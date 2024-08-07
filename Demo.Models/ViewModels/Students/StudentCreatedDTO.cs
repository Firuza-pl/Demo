using Demo.Domain.Attributes;
namespace Demo.Domain.ViewModels.Students;
public class StudentCreatedDTO
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    [CustomDateTimeConverter(ErrorMessage = "DateOfBirth must be greater than today"))]
    public DateTime DateOfBirth { get; set; }
    public string? Email { get; set; }

    [PhoneNumberFormat]
    public string? PhoneNumber { get; set; }
}
