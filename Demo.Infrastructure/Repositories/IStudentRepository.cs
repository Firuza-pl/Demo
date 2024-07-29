using Demo.Domain.AggregatesModel;
namespace Demo.Infrastructure.Repositories;
public interface IStudentRepository
{
    Task<IEnumerable<Student>> GetAllStudentsAsync();
    Task<Student> GetByIdAsync(int id);
    Task<int> AddStudentAsync(Student student);
    Task<int> UpdateStudentAsync(Student student);
    Task<bool> DeleteStudentAsync(int id);
}
