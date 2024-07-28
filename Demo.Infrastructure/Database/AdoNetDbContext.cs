using System.Data.SqlClient;

namespace Demo.Infrastructure.Database;
public class AdoNetDbContext
{
    private readonly string _connectionString;

    public AdoNetDbContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    public SqlConnection GetConnection()
    {
        return new SqlConnection(_connectionString);
    }

    public void OpenConnection()
    {
        using (var connection = GetConnection())
        {
            connection.Open();
        }
    }
}