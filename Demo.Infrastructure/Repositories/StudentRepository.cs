using Demo.Domain.AggregatesModel;
using Demo.Infrastructure.Database;
using System.Data.SqlClient;

namespace Demo.Infrastructure.Repositories;
public class StudentRepository : IStudentRepository
{
    private readonly AdoNetDbContext _context;

    public StudentRepository(AdoNetDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Student>> GetAllStudentsAsync()
    {
        var students = new List<Student>();

        using var conn = _context.GetConnection();
        await conn.OpenAsync();
        try
        {
            using var command = new SqlCommand("Select * from Students", conn);

            using var dataReader = await command.ExecuteReaderAsync();

            while (await dataReader.ReadAsync()) // if there's at least one row
            {
                Student student = new();

                student.AddData(
                    dataReader.GetInt32(0), // column name 
                    dataReader.GetString(1),
                    dataReader.GetString(2)
                );

                students.Add(student);
            }

            return students;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            throw;
        }
        // No need to manually close the connection , it's handled by using statement
    }
    public async Task<Student> GetByIdAsync(int studentId)
    {
        try
        {
            using var conn = _context.GetConnection();
            await conn.OpenAsync();

            // No need for a transaction for read operations
            using var command = new SqlCommand("SELECT * FROM Students WHERE StudentId = @StudentId", conn);
            command.Parameters.AddWithValue("@StudentId", studentId);

            using var dataReader = await command.ExecuteReaderAsync();

            Student student = null;

            if (await dataReader.ReadAsync()) // if there's at least one row
            {
                student = new Student();
                student.AddData(
                    dataReader.GetInt32(0), // column name 
                    dataReader.GetString(1),
                    dataReader.GetString(2)
                );
            }

            return student;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            throw;
        }
        finally
        {
            // No need to manually close the connection , it's handled by using statement
        }
    }
    public async Task<int> AddStudentAsync(Student student)
    {
        try
        {
            using var conn = _context.GetConnection();
            await conn.OpenAsync();

            using var transaction = conn.BeginTransaction();

            try
            {

                using var command = new SqlCommand("INSERT INTO Students (FirstName, LastName) VALUES (@FirstName, @LastName)", conn, transaction);
                command.Parameters.AddWithValue("@FirstName", student.FirstName);
                command.Parameters.AddWithValue("@LastName", student.LastName);

                int rowsAffected = await command.ExecuteNonQueryAsync();

                if (rowsAffected > 0)
                {
                    await transaction.CommitAsync();
                    return rowsAffected;
                }
                else
                {
                    await transaction.RollbackAsync();
                    return 0;
                }

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                Console.WriteLine($"Error: {ex.Message}");
                throw; //is used to throw exceptions in general, not just ArgumentNullException
            }
            finally
            {
                conn.Close(); //explicitly call, when managing the connection lifecycle more manually, 
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            throw;
        }
    }
    public async Task<int> UpdateStudentAsync(Student student)
    {
        try
        {
            using var conn = _context.GetConnection();
            await conn.OpenAsync();

            using var transaction = conn.BeginTransaction();

            try
            {

                using var command = new SqlCommand("UPDATE Students SET FirstName = @FirstName, LastName = @LastName WHERE StudentId = @StudentId", conn, transaction);
                command.Parameters.AddWithValue("@StudentId", student.StudentId);
                command.Parameters.AddWithValue("@FirstName", student.FirstName);
                command.Parameters.AddWithValue("@LastName", student.LastName);

                int rowsAffected = await command.ExecuteNonQueryAsync();

                if (rowsAffected > 0)
                {
                    await transaction.CommitAsync();
                    return rowsAffected;
                }
                else
                {
                    await transaction.RollbackAsync();
                    return 0;
                }

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                Console.WriteLine($"Error: {ex.Message}");
                throw; //is used to throw exceptions in general, not just ArgumentNullException
            }
            finally
            {
                conn.Close();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            throw;
        }
    }
    public async Task<bool> DeleteStudentAsync(int StudentId)
    {
        try
        {
            using var conn = _context.GetConnection();
            await conn.OpenAsync(); //before started transaction

            await conn.OpenAsync();

            using var transaction = conn.BeginTransaction();

            try
            {

                using var deleteEnrollmentsCommand = new SqlCommand("DELETE FROM Enrollments WHERE StudentId = @StudentId", conn, transaction);
                deleteEnrollmentsCommand.Parameters.AddWithValue("@StudentId", StudentId);

                await deleteEnrollmentsCommand.ExecuteNonQueryAsync();


                using var deleteStudentCommand = new SqlCommand("DELETE FROM Students WHERE StudentId = @StudentId", conn, transaction);
                deleteStudentCommand.Parameters.AddWithValue("@StudentId", StudentId);

                int rowsAffected = await deleteStudentCommand.ExecuteNonQueryAsync();

                if (rowsAffected > 0)
                {
                    await transaction.CommitAsync(); // if all commands execute successfully   
                    return true;
                }
                else
                {
                    await transaction.RollbackAsync();
                    return false;
                }
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
            finally
            {
                conn.Close();
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            throw;
        }
    }


}
