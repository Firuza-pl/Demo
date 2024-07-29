using Demo.Domain.AggregatesModel;
using Demo.Domain.ViewModels.Students;
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

    public Student GetStudent(int id)
    {
        return _studentRepository.GetById(id);
    }
    public void AddStudent(StudentCreatedDTO studentCreated)
    {
        if (studentCreated is { })
        {
            Student student = new();
            student.AddData(0, studentCreated.FirstName, studentCreated.LastName);
            _studentRepository.AddStudent(student);
        }
    }
    public void UpdateStudent(int id, StudentUpdateDTO studentUpdate)
    {
        Student existingStudent = _studentRepository.GetById(id);

        if (existingStudent is { })
        {
            existingStudent.UpdateData(studentUpdate.FirstName,studentUpdate.LastName);
            _studentRepository.UpdateStudent(existingStudent);
        }
    }

    public async Task<bool> DeleteStudent(int studentId)
    {
       return await _studentRepository.DeleteStudent(studentId);
    }
}
