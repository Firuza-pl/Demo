using Demo.Domain.AggregatesModel;
using Demo.Domain.ViewModels.Students;
namespace Demo.Infrastructure.Services;
public interface IStudentService
{
    public IEnumerable<Student> GetAllStudents();
    public Student GetStudent(int id);
    public void AddStudent(StudentCreatedDTO studentCreated);
    void UpdateStudent(int studentId, StudentUpdateDTO studentUpdate);
    void DeleteStudent(int studentId);
}
