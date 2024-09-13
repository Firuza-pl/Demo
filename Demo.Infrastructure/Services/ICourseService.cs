using Demo.Domain.AggregatesModel;
using Demo.Domain.ViewModels.Courses;
namespace Demo.Infrastructure.Services;
public interface ICourseService
{
    Task<IEnumerable<Course>> GetAllCoursesAsync();
    Task<Course> GetCourseAsync(int id);
    Task<int> AddCourseAsync(CourseCreatedDTO studentCreated);
    Task<int> UpdateCourseAsync(int studentId, CourseUpdateDTO studentUpdate);
    Task<bool> DeleteCourseAsync(int studentId);
}
