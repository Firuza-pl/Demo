namespace Demo.Domain.AggregatesModel;
public class Student
{
    public int StudentId { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public ICollection<Enrollment> Enrollments { get; set; }

    public Student(int studentId, string firstName, string lastName)
    {
        StudentId = studentId;
        FirstName = firstName;
        LastName = lastName;
        Enrollments = new List<Enrollment>();
    }

    public void UpdateStudentDetails(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }
}
