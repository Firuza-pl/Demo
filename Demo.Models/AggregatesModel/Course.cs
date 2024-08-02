namespace Demo.Domain.AggregatesModel;
public class Course
{
    public int CourseId { get; private set; }
    public string CourseName { get; private set; }
    public int Credits { get; private set; }
    public bool IsActive { get; private set; }

    private ICollection<Enrollment> _enrollments;
    public IReadOnlyCollection<Enrollment> Enrollments => (IReadOnlyCollection<Enrollment>)_enrollments;

    public Course()
    {
        _enrollments = new List<Enrollment>();
    }

    public void UpdateCourseDetails(string name, int credits)
    {
        CourseName = name;
        Credits = credits;
    }

    public void AddData(int courseId, string courseName, int credits, bool isActive)
    {
        CourseId = courseId;
        CourseName = courseName;
        Credits = credits;
        IsActive = isActive;
    }
    public void UpdateData(string courseName, int credits, bool isActive)
    {
        CourseName = courseName;
        Credits = credits;
        IsActive = isActive;
    }
}

