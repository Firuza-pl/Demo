using Demo.Domain.AggregatesModel;
namespace Demo.Infrastructure.Repositories;
public interface IStudentRepository
{
    public IEnumerable<Student> GetAllStudents();
    public Student GetById(int id);
    public void AddStudent(Student student);
    void UpdateStudent(Student student);
    Task<bool> DeleteStudent(int id);
}
