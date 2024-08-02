using System.ComponentModel.DataAnnotations;
namespace Demo.Domain.Attributes;

 [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class EmailFormatAttribute : ValidationAttribute
{
    public EmailFormatAttribute() : base("Invalid email format.")
    {
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var email = value as string;
        if (email != null && !IsValidEmail(email))
        {
            return new ValidationResult(ErrorMessage);
        }
        return ValidationResult.Success;
    }

    private bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}