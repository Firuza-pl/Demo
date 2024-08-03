using Demo.Domain.AggregatesModel;
using Demo.Infrastructure.Database;
using System.Data.SqlClient;
namespace Demo.Infrastructure.Repositories;
public class CourseRepository : ICourseRepository
{
    private readonly AdoNetDbContext _context;

    public CourseRepository(AdoNetDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Course>> GetAllCoursesAsync()
    {
        var courses = new List<Course>();

        using var conn = _context.GetConnection();
        await conn.OpenAsync();

        try
        {
            using var command = new SqlCommand("SELECT * FROM Courses WHERE IsActive = @IsActive", conn);
            command.Parameters.AddWithValue("@IsActive", true);

            using var dataReader = await command.ExecuteReaderAsync();

            if (!dataReader.HasRows)
            {
                Console.WriteLine("No active course found.");
                return courses;
            }

            while (await dataReader.ReadAsync()) // Process each row
            {
                var course = new Course();

                course.AddData(
                    dataReader.GetInt32(0),
                    dataReader.GetString(1),
                    dataReader.GetInt32(2),
                    dataReader.GetBoolean(3)
                );

                courses.Add(course);
            }

            return courses;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            throw;
        }
        // No need to manually close the connection , it's handled by using statement
    }
    public async Task<Course> GetByIdAsync(int courseId)
    {
        try
        {
            using var conn = _context.GetConnection();
            await conn.OpenAsync();

            // No need for a transaction for read operations
            using var command = new SqlCommand("SELECT * FROM Courses WHERE CourseId = @CourseId AND IsActive = @IsActive", conn);
            command.Parameters.AddWithValue("@CourseId", courseId);
            command.Parameters.AddWithValue("@IsActive", true);

            using var dataReader = await command.ExecuteReaderAsync();

            if (!dataReader.HasRows)
            {
                Console.WriteLine("No active course found with the provided ID.");
                return null;
            }

            Course course = null;

            if (await dataReader.ReadAsync()) // if there's at least one row
            {
                course = new Course();
                course.AddData(
           dataReader.GetInt32(0),
                    dataReader.GetString(1),
                    dataReader.GetInt32(2),
                    dataReader.GetBoolean(3)
                );
            }

            return course;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            throw;
        }
        // No need to manually close the connection , it's handled by using statement
    }
    public async Task<int> AddCoursesAsync(Course course)
    {
        try
        {
            using var conn = _context.GetConnection();
            await conn.OpenAsync();

            using var transaction = conn.BeginTransaction();

            try
            {

                using var command = new SqlCommand("INSERT INTO Courses (CourseName, Credits, IsActive) VALUES (@CourseName, @Credits, @IsActive)", conn, transaction);
                command.Parameters.AddWithValue("@CourseName", course.CourseName);
                command.Parameters.AddWithValue("@Credits", course.Credits);
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
    public async Task<int> UpdateCoursesAsync(Course course)
    {
        try
        {
            using var conn = _context.GetConnection();
            await conn.OpenAsync();

            using var transaction = conn.BeginTransaction();

            try
            {

                using var command = new SqlCommand("UPDATE Courses SET CourseName = @CourseName, Credits = @Credits WHERE CourseId = @CourseId and IsActive = @IsActive", conn, transaction);
                command.Parameters.AddWithValue("@CourseId", course.CourseId);
                command.Parameters.AddWithValue("@Credits", course.Credits);
                command.Parameters.AddWithValue("@CourseName", course.CourseName);
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
    public async Task<bool> DeleteCoursesAsync(int CourseId)
    {
        try
        {
            using var conn = _context.GetConnection();
            await conn.OpenAsync(); //before started transaction

            using var transaction = conn.BeginTransaction();

            try
            {

                using var deleteStudentCommand = new SqlCommand("UPDATE Courses SET IsActive = @IsActive WHERE CourseId = @CourseId", conn, transaction);
                deleteStudentCommand.Parameters.AddWithValue("@CourseId", CourseId);
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

