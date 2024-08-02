using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
namespace Demo.Domain.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class PhoneNumberFormatAttribute : ValidationAttribute
{
    public PhoneNumberFormatAttribute() : base("Invalid phone number format. It should start with +994 followed by 9 digits.")
    {
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var phoneNumber = value as string;
        if (phoneNumber != null && !IsValidPhoneNumber(phoneNumber))
        {
            return new ValidationResult(ErrorMessage);
        }
        return ValidationResult.Success;
    }

    private bool IsValidPhoneNumber(string phoneNumber)
    {
        // Regex for +994 followed by 9 digits
        var regex = new Regex(@"^\+994\d{9}$");  //for Azerbaijan
        return regex.IsMatch(phoneNumber);
    }
}
