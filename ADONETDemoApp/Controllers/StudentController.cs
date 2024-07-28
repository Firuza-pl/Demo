using Demo.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Demo.Domain.ViewModels.Students;

namespace ADONETDemoApp.Controllers;
public class StudentController : Controller
{
    private readonly IStudentService _studentService;
    public StudentController(IStudentService studentService)
    {
        _studentService = studentService;
    }

    [HttpGet]
    [Route("Index")]
    public IActionResult Index()
    {
        var students = _studentService.GetAllStudents().Select(s => new StudentAllDTO
        {
            StudentId = s.StudentId,
            FirstName = s.FirstName,
            LastName = s.LastName
        });

        return View(students);
    }
}
