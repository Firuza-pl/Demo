using Demo.Domain.AggregatesModel;
using Demo.Domain.ViewModels.Courses;
using Demo.Infrastructure.Repositories;

namespace Demo.Infrastructure.Services;
internal class CourseService : ICourseService
{
    private readonly ICourseRepository _courseRepository;

    public CourseService(ICourseRepository courseRepository)
    {
        _courseRepository = courseRepository;
    }
    public async Task<IEnumerable<Course>> GetAllCoursesAsync()
    {
        return await _courseRepository.GetAllCoursesAsync();
    }

    public async Task<Course> GetCourseAsync(int id)
    {
        return await _courseRepository.GetByIdAsync(id);
    }
    public async Task<int> AddCourseAsync(CourseCreatedDTO courseCreated)
    {
        if (courseCreated is { })
        {
            Course course = new();
            course.AddData(0, courseCreated.CourseName, courseCreated.Credits, true);
            await _courseRepository.AddCoursesAsync(course);
        }

        return 0;
    }
    public async Task<int> UpdateCourseAsync(int id, CourseUpdateDTO courseUpdate)
    {
        Course existingcourse = await _courseRepository.GetByIdAsync(id);

        if (existingcourse is { })
        {
            existingcourse.UpdateData(courseUpdate.CourseName, courseUpdate.Credits,  true);
            await _courseRepository.UpdateCoursesAsync(existingcourse);
        }
        return 0;
    }

    public async Task<bool> DeleteCourseAsync(int courseId)
    {
        return await _courseRepository.DeleteCoursesAsync(courseId);
    }
}