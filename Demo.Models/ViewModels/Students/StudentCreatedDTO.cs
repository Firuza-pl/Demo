using Demo.Domain.Attributes;
using System.Text.Json.Serialization;
namespace Demo.Domain.ViewModels.Students;
public class StudentCreatedDTO
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    [JsonConverter(typeof(CustomDateTimeConverter))]
    public DateTime DateOfBirth { get; set; }

    [EmailFormat]
    public string? Email { get; set; }

    [PhoneNumberFormat]
    public string? PhoneNumber { get; set; }
}
