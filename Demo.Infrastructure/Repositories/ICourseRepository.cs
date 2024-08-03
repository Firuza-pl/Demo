using Demo.Domain.AggregatesModel;
namespace Demo.Infrastructure.Repositories;
public interface ICourseRepository
{
    Task<IEnumerable<Course>> GetAllCoursesAsync();
    Task<Course> GetByIdAsync(int id);
    Task<int> AddCoursesAsync(Course student);
    Task<int> UpdateCoursesAsync(Course student);
    Task<bool> DeleteCoursesAsync(int id);
}
