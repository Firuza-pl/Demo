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

    public Student GetById(int StudentId)
    {
        Student student = null;

        using var conn = _context.GetConnection();
        using var command = new SqlCommand("SELECT * FROM Students WHERE StudentId = @StudentId ", conn);
        command.Parameters.AddWithValue("@StudentId", StudentId); //prm name must match column name
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
        using var command = new SqlCommand("INSERT INTO Students (FirstName, LastName) VALUES (@FirstName, @LastName)", conn);
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

    public async Task<bool> DeleteStudent(int StudentId)
    {
        try
        {
            using var conn = _context.GetConnection();
            conn.Open(); //before started transaction

            using var transaction = conn.BeginTransaction();

            try
            {

                using var deleteEnrollmentsCommand = new SqlCommand("DELETE FROM Enrollments WHERE StudentId = @StudentId", conn, transaction);
                deleteEnrollmentsCommand.Parameters.AddWithValue("@StudentId", StudentId);
                deleteEnrollmentsCommand.ExecuteNonQuery();


                using var deleteStudentCommand = new SqlCommand("DELETE FROM Students WHERE StudentId = @StudentId", conn, transaction);
                deleteStudentCommand.Parameters.AddWithValue("@StudentId", StudentId);
                int rowsAffected = deleteStudentCommand.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    transaction.Commit(); // if all commands execute successfully   
                    return true; //deleted
                }
                else
                {
                    transaction.Rollback();
                    return false;
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
            finally
            {
                conn.Close();
            }

        }
        catch (Exception)
        {

            throw;
        }
    }


}
