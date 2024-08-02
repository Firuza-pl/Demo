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
    public async Task<IEnumerable<Student>> GetAllStudentsAsync()
    {
        return await _studentRepository.GetAllStudentsAsync();
    }

    public async Task<Student> GetStudentAsync(int id)
    {
        return await _studentRepository.GetByIdAsync(id);
    }
    public async Task<int> AddStudentAsync(StudentCreatedDTO studentCreated)
    {
        if (studentCreated is { })
        {
            Student student = new();
            student.AddData(0, studentCreated.FirstName, studentCreated.LastName, studentCreated.DateOfBirth, studentCreated.Email, studentCreated.PhoneNumber, true);
            await _studentRepository.AddStudentAsync(student);
        }

        return 0;
    }
    public async Task<int> UpdateStudentAsync(int id, StudentUpdateDTO studentUpdate)
    {
        Student existingStudent = await _studentRepository.GetByIdAsync(id);

        if (existingStudent is { })
        {
            existingStudent.UpdateData(studentUpdate.FirstName, studentUpdate.LastName, studentUpdate.DateOfBirth, studentUpdate.Email, studentUpdate.PhoneNumber, true);
            await _studentRepository.UpdateStudentAsync(existingStudent);
        }
        return 0;
    }

    public async Task<bool> DeleteStudentAsync(int studentId)
    {
        return await _studentRepository.DeleteStudentAsync(studentId);
    }
}
