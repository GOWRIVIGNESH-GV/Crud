using System.Data;
using Oracle.ManagedDataAccess.Client;

public class DBContext
{
    private readonly string _connectionString;

    public DBContext(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("OracleDb");
    }

    public IDbConnection CreateConnection()
    {
        var connection = new OracleConnection(_connectionString);

        try
        {
            connection.Open();
            Console.WriteLine("Oracle connection opened successfully.");
        }
        catch (OracleException ex)
        {
            Console.WriteLine($"Oracle Exception: {ex.Message}");
            throw new Exception($"Failed to open Oracle connection. {ex.Message} {_connectionString}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"General Exception: {ex.Message}");
            throw new Exception($"An error occurred while opening Oracle connection. {ex.Message} {_connectionString}");
        }

        return connection;
    }

    public async Task<OracleConnection> CreateConnectionAsync()
    {
        var connection = new OracleConnection(_connectionString);

        try
        {
            await connection.OpenAsync();
            Console.WriteLine("Oracle async connection opened successfully.");
        }
        catch (OracleException ex)
        {
            Console.WriteLine($"Oracle Exception: {ex.Message}");
            throw new Exception("Failed to open Oracle connection.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"General Exception: {ex.Message}");
            throw new Exception("An error occurred while opening Oracle connection.");
        }

        return connection;
    }
}
