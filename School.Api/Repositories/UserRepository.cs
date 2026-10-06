using System.Data;
using Microsoft.Data.SqlClient;
using SchoolApi.Models;
using System.Threading.Tasks;

namespace SchoolApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("SELECT * FROM Users WHERE UserName = @UserName", conn);
            cmd.Parameters.AddWithValue("@UserName", username);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new User
                {
                    UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                    UserName = reader.GetString(reader.GetOrdinal("UserName")),
                    PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                    Role = reader.GetString(reader.GetOrdinal("Role")),
                    Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email")),
                    CreatedAt = reader.IsDBNull(reader.GetOrdinal("CreatedAt"))
                        ? DateTime.MinValue
                        : reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
                };
            }

            return null;
        }


        public async Task<User?> GetByIdAsync(int userId)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new SqlCommand("SELECT * FROM Users WHERE UserId = @UserId", conn);
            cmd.Parameters.AddWithValue("@UserId", userId);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return MapUser(reader);
            }
            return null;
        }

        public async Task<int> AddAsync(User user)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new SqlCommand(@"
                INSERT INTO Users (UserName, PasswordHash, Role, Email)
                OUTPUT INSERTED.UserId
                VALUES (@UserName, @PasswordHash, @Role, @Email)", conn);

            cmd.Parameters.AddWithValue("@UserName", user.UserName);
            cmd.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
            cmd.Parameters.AddWithValue("@Role", user.Role);
            cmd.Parameters.AddWithValue("@Email", (object?)user.Email ?? DBNull.Value);

            var id = (int)await cmd.ExecuteScalarAsync();
            return id;
        }

        public async Task<bool> ExistsAsync(string username)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new SqlCommand("SELECT COUNT(1) FROM Users WHERE UserName = @UserName", conn);
            cmd.Parameters.AddWithValue("@UserName", username);

            var count = (int)await cmd.ExecuteScalarAsync();
            return count > 0;
        }

        private User MapUser(SqlDataReader reader)
        {
            return new User
            {
                UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                UserName = reader.GetString(reader.GetOrdinal("UserName")),
                PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                Role = reader.GetString(reader.GetOrdinal("Role")),
                Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email")),
                CreatedAt = reader.IsDBNull(reader.GetOrdinal("CreatedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
            };
        }
    }
}
