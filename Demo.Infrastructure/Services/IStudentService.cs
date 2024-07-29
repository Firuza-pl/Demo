using Demo.Domain.AggregatesModel;
using Demo.Domain.ViewModels.Students;
namespace Demo.Infrastructure.Services;
public interface IStudentService
{
    Task<IEnumerable<Student>> GetAllStudentsAsync();
    Task<Student> GetStudentAsync(int id);
    Task<int> AddStudentAsync(StudentCreatedDTO studentCreated);
    Task<int> UpdateStudentAsync(int studentId, StudentUpdateDTO studentUpdate);
    Task<bool> DeleteStudentAsync(int studentId);
}
