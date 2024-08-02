using Demo.Domain.AggregatesModel;
using Demo.Infrastructure.Database;
using System;
using System.Data;
using System.Data.Common;
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
            using var command = new SqlCommand("SELECT * FROM STUDENTS WHERE IsActive = @IsActive", conn);
            command.Parameters.AddWithValue("@IsActive", true);

            using var dataReader = await command.ExecuteReaderAsync();

            if (!dataReader.HasRows) 
            {
                Console.WriteLine("No active students found.");
                return students; 
            }

            while (await dataReader.ReadAsync()) // Process each row
            {
                var student = new Student();

                student.AddData(
                    dataReader.GetInt32(0), // Assuming column index matches the property order
                    dataReader.GetString(1),
                    dataReader.GetString(2),
                    dataReader.GetDateTime(3),
                    dataReader.GetString(4),
                    dataReader.GetString(5),
                    dataReader.GetBoolean(6)
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
            using var command = new SqlCommand("SELECT * FROM Students WHERE StudentId = @StudentId AND IsActive = @IsActive", conn);
            command.Parameters.AddWithValue("@StudentId", studentId);
            command.Parameters.AddWithValue("@IsActive", true);

            using var dataReader = await command.ExecuteReaderAsync();

            if (!dataReader.HasRows)
            {
                Console.WriteLine("No active student found with the provided ID.");
                return null; 
            }

            Student student = null;

            if (await dataReader.ReadAsync()) // if there's at least one row
            {
                student = new Student();
                    student.AddData(
                        dataReader.GetInt32(0), // column
                        dataReader.GetString(1),
                        dataReader.GetString(2),
                        dataReader.GetDateTime(3),
                        dataReader.GetString(4),
                        dataReader.GetString(5),
                        dataReader.GetBoolean(6)
                    );
            }

            return student;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            throw;
        }
        // No need to manually close the connection , it's handled by using statement
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

                using var command = new SqlCommand("INSERT INTO Students (FirstName, LastName, DateOfBirth, Email, PhoneNumber, IsActive) VALUES (@FirstName, @LastName, @DateOfBirth, @Email, @PhoneNumber, @IsActive)", conn, transaction);
                command.Parameters.AddWithValue("@FirstName", student.FirstName);
                command.Parameters.AddWithValue("@LastName", student.LastName);
                command.Parameters.AddWithValue("@DateOfBirth", student.DateOfBirth);
                command.Parameters.AddWithValue("@Email", student.Email);
                command.Parameters.AddWithValue("@PhoneNumber", student.PhoneNumber);
                command.Parameters.AddWithValue("@IsActive", true);

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

                using var command = new SqlCommand("UPDATE Students SET FirstName = @FirstName, LastName = @LastName WHERE StudentId = @StudentId and IsActive = @IsActive", conn, transaction);
                command.Parameters.AddWithValue("@StudentId", student.StudentId);
                command.Parameters.AddWithValue("@FirstName", student.FirstName);
                command.Parameters.AddWithValue("@LastName", student.LastName);
                command.Parameters.AddWithValue("@DateOfBirth", student.DateOfBirth);
                command.Parameters.AddWithValue("@Email", student.Email);
                command.Parameters.AddWithValue("@PhoneNumber", student.PhoneNumber);
                command.Parameters.AddWithValue("@IsActive", true);

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

            using var transaction = conn.BeginTransaction();

            try
            {

                using var deleteStudentCommand = new SqlCommand("UPDATE Students SET IsActive = @IsActive WHERE StudentId = @StudentId", conn, transaction);
                deleteStudentCommand.Parameters.AddWithValue("@StudentId", StudentId);
                deleteStudentCommand.Parameters.AddWithValue("@IsActive", false);  //instead of deleting data we change status

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
