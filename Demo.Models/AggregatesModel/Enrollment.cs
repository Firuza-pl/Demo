namespace Demo.Domain.AggregatesModel;
public class Enrollment
{
    public int StudentId { get; private set; }
    public Student Student { get; private set; }
    public int CourseId { get; private set; }
    public Course Course { get; private set; }
    public DateTime EnrollmentDate { get; private set; }
    public string Grade { get; set; }

    public Enrollment(int studentId, Student student, int courseId, Course course, DateTime enrollmentDate, string grade)
    {
        StudentId = studentId;
        Student = student;
        CourseId = courseId;
        Course = course;
        EnrollmentDate = enrollmentDate;
        Grade = grade;
    }

    public void AddData(int studentId, int courseId, DateTime enrollmentDate, string grade)
    {
        StudentId = studentId;
        CourseId = courseId;
        EnrollmentDate=enrollmentDate;
        Grade = grade;

    }

    public void UpdateData(int studentId, int courseId, DateTime enrollmentDate, string grade) {

        StudentId = studentId;
        CourseId = courseId;
        EnrollmentDate = enrollmentDate;
        Grade = grade;
    }
}
