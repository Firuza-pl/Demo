namespace Demo.Domain.AggregatesModel;
public class Course
{
    public int CourseId { get; private set; }
    public string CourseName { get; private set; }
    public int Credits { get; private set; }
    public ICollection<Enrollment> Enrollments { get; set; }

    public Course(int courseId, string name, int credits)
    {
        CourseId = courseId;
        CourseName = name;
        Credits = credits;
        Enrollments = new List<Enrollment>();
    }

    public void UpdateCourseDetails(string name, int credits)
    {
        CourseName = name;
        Credits = credits;
    }
}

