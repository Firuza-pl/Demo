namespace Demo.Domain.AggregatesModel;
public class Enrollment
{
    public int StudentId { get; private set; }
    public Student Student { get; private set; }
    public int CourseId { get; private set; }
    public Course Course { get; private set; }
    public DateTime EnrollmentDate { get; private set; }

    public Enrollment(int studentId, Student student, int courseId, Course course, DateTime enrollmentDate)
    {
        StudentId = studentId;
        Student = student;
        CourseId = courseId;
        Course = course;
        EnrollmentDate = enrollmentDate;
    }
}
