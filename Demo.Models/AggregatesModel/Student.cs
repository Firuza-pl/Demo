namespace Demo.Domain.AggregatesModel;
public class Student
{
    public int StudentId { get; private set; }
    public string? FirstName { get; private set; }
    public string? LastName { get; private set; }

    private ICollection<Enrollment> _enrollments;
    public IReadOnlyCollection<Enrollment> Enrollments => (IReadOnlyCollection<Enrollment>)_enrollments;

    public Student()
    {
        _enrollments = new List<Enrollment>();
    }

    public void AddData(int studentId, string firstName, string lastName)
    {
        StudentId = studentId;
        FirstName = firstName;
        LastName = lastName;
    }
    public void UpdateData(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

}
