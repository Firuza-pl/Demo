using System.ComponentModel.DataAnnotations;
namespace Demo.Domain.Attributes;
public class CustomDateTimeConverter : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is DateTime dateTime)
        {
            return dateTime > DateTime.Now;
        }
        return false;
    }
}


