using Demo.Domain.AggregatesModel;
namespace Demo.Infrastructure.Services;
public interface IStudentService
{
    public IEnumerable<Student> GetAllStudents();
}
