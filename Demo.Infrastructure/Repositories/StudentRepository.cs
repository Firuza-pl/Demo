using Demo.Domain.AggregatesModel;
using Demo.Infrastructure.Database;

namespace Demo.Infrastructure.Repositories;
public class StudentRepository : IStudentRepository
{
    private readonly AdoNetDbContext _context;

    public StudentRepository(AdoNetDbContext context)
    {
        _context = context;
    }
    public IEnumerable<Student> GetAllStudents()
    {
        var students = new List<Student>();
        return students;
    }
}
