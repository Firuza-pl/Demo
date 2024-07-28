using Demo.Domain.AggregatesModel;
using Demo.Infrastructure.Database;
using System.Data;
using System.Data.SqlClient;

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

        using var conn = _context.GetConnection(); //new SqlConnection(conn);

        using var command = new SqlCommand("Select * from Students", conn);  //MUST TO DO: change to stored procedure
        //cmd.CommandType = CommandType.StoredProcedure;
        conn.Open();

        using var dataReader = command.ExecuteReader();
        while (dataReader.Read())
        {
            Student student = new Student();
            student.AddData(dataReader.GetInt32(0), dataReader.GetString(1), dataReader.GetString(2));

            students.Add(student);
        }
        conn.Close();
        return students;
    }

    public Student GetById(int id)
    {
        Student student = null;

        using var conn = _context.GetConnection();
        using var command = new SqlCommand("SELECT * FROM Students WHERE StudentId = @id ", conn);
        command.Parameters.AddWithValue("@StudentId", id);
        conn.Open();

        using var dataReader = command.ExecuteReader();
        while (dataReader.Read())
        {
            student = new Student();
            student.AddData(dataReader.GetInt32(0), dataReader.GetString(1), dataReader.GetString(2));
        }

        conn.Close();
        return student;
    }

    public void AddStudent(Student student)
    {
        using var conn = _context.GetConnection();
        using var command = new SqlCommand("INSERT INTO Students (StudentId, FirstName, LastName) VALUES (@StudentId, @FirstName, @LastName", conn);
        command.Parameters.AddWithValue("@FirstName", student.FirstName);
        command.Parameters.AddWithValue("@LastName", student.LastName);

        conn.Open();
        command.ExecuteNonQuery();
        conn.Close();
    }
    public void UpdateStudent(Student student)
    {
        using var conn = _context.GetConnection();
        using var command = new SqlCommand("UPDATE Students SET FirstName = @FirstName, LastName = @LastName WHERE StudentId = @StudentId", conn);
        command.Parameters.AddWithValue("@StudentId", student.StudentId);
        command.Parameters.AddWithValue("@FirstName", student.FirstName);
        command.Parameters.AddWithValue("@LastName", student.LastName);

        conn.Open();
        command.ExecuteNonQuery();
        conn.Close();
    }

    public void DeleteStudent(int id) {

        using var conn = _context.GetConnection();
        using var command = new SqlCommand("DELETE FROM Students WHERE StudentId = @StudentId", conn);
        command.Parameters.AddWithValue("@StudentId", id);

        conn.Open();
        command.ExecuteNonQuery();
        conn.Close();
    }


}
