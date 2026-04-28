using _5026CMD_GroupProject.Models;
using Microsoft.Data.SqlClient;

namespace _5026CMD_GroupProject.Services
{
    public class DatabaseService
    {
        private readonly string? _connectionString;

        // This is the Constructor - it must have the same name as the Class
        public DatabaseService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<User?> ValidateLogin(string username, string password)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = "SELECT * FROM Users WHERE Username = @user AND Password = @pass";
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@user", username);
            command.Parameters.AddWithValue("@pass", password);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new User
                {
                    UserID = (int)reader["UserID"],
                    Username = reader["Username"].ToString()!,
                    Role = reader["Role"].ToString()!
                };
            }
            return null; // Login failed
        }
    }
}