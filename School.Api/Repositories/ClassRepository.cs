using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using SchoolApi.Models;
using SchoolApi.Repositories;

namespace SchoolApi.Repositories
{
	public class ClassRepository : IClassRepository
	{
        private readonly string _connectionString;

        public ClassRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<Class>> GetAllAsync()
        {
            var classes = new List<Class>();

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("GetAllClasses", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                classes.Add(new Class
                {
                    ClassId = reader.GetInt32(reader.GetOrdinal("ClassId")),
                    Name = reader.IsDBNull(reader.GetOrdinal("Name"))
                           ? string.Empty
                           : reader.GetString(reader.GetOrdinal("Name")),
                    Section = reader.IsDBNull(reader.GetOrdinal("Section"))
                              ? string.Empty
                              : reader.GetString(reader.GetOrdinal("Section"))
                });
            }

            return classes;
        }

        public async Task<bool> ExistsAsync(int classId)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("SELECT COUNT(1) FROM Classes WHERE ClassId = @ClassId", conn);
            cmd.Parameters.AddWithValue("@ClassId", classId);

            await conn.OpenAsync();
            var result = (int)await cmd.ExecuteScalarAsync();
            return result > 0;
        }
    }
}
