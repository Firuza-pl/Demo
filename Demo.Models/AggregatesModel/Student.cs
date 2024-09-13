using Demo.Domain.Attributes;
using System.Text.Json.Serialization;
namespace Demo.Domain.AggregatesModel;
public class Student
{
    public int StudentId { get; private set; }
    public string? FirstName { get; private set; }
    public string? LastName { get; private set; }
    public DateTime DateOfBirth { get; set; }

    [EmailFormat]
    public string Email { get; set; }

    [PhoneNumberFormat]
    public string PhoneNumber { get; set; }
    public bool IsActive { get; private set; }

    private ICollection<Enrollment> _enrollments;
    public IReadOnlyCollection<Enrollment> Enrollments => (IReadOnlyCollection<Enrollment>)_enrollments;

    public Student()
    {
        _enrollments = new List<Enrollment>();
    }

    public void AddData(int studentId, string firstName, string lastName, DateTime dateOfBirth, string email, string phonenumber, bool isActive)
    {
        StudentId = studentId;
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
        Email = email;
        PhoneNumber = phonenumber;
        IsActive = isActive;
    }
    public void UpdateData(string firstName, string lastName, DateTime dateOfBirth, string email, string phonenumber, bool isActive)
    {
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
        Email = email;
        PhoneNumber = phonenumber;
        IsActive = isActive;
    }

}
