using Demo.Domain.AggregatesModel;
using Demo.Infrastructure.Repositories;
namespace Demo.Infrastructure.Services;
internal class StudentService : IStudentService
{

    private readonly IStudentRepository _studentRepository;

    public StudentService(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }
    public IEnumerable<Student> GetAllStudents()
    {
        return _studentRepository.GetAllStudents();
    }
}
