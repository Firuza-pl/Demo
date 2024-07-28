using Demo.Domain.AggregatesModel;
namespace Demo.Infrastructure.Repositories;
public interface IStudentRepository
{
    public IEnumerable<Student> GetAllStudents();
}
