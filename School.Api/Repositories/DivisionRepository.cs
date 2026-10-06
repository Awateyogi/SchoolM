using Microsoft.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using SchoolApi.Models;
using SchoolApi.Repositories;

namespace SchoolApi.Repositories
{
    public class DivisionRepository : IDivisionRepository
    {
        private readonly string _connectionString;

        public DivisionRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

         public async Task<IEnumerable<Division>> GetAllAsync()
        {
            var divisions = new List<Division>();
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("sp_GetAllDivisions", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                divisions.Add(new Division
                {
                    DivisionId = reader.GetInt32(reader.GetOrdinal("DivisionId")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    ClassId = reader.GetInt32(reader.GetOrdinal("ClassId")),
                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
                });
            }
            return divisions;
        }

        public async Task<Division?> GetByIdAsync(int id)
        {
            Division? division = null;
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("sp_GetDivisionById", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@DivisionId", id);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                division = new Division
                {
                    DivisionId = reader.GetInt32(reader.GetOrdinal("DivisionId")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    ClassId = reader.GetInt32(reader.GetOrdinal("ClassId")),
                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
                };
            }
            return division;
        }

        public async Task<int> AddAsync(Division division)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("sp_AddDivision", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Name", division.Name);
            cmd.Parameters.AddWithValue("@ClassId", division.ClassId);

            await conn.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }

        public async Task<bool> UpdateAsync(Division division)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("sp_UpdateDivision", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@DivisionId", division.DivisionId);
            cmd.Parameters.AddWithValue("@Name", division.Name);
            cmd.Parameters.AddWithValue("@ClassId", division.ClassId);

            await conn.OpenAsync();
            var rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }

        // ✅ This was missing
        public async Task<bool> DeleteAsync(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("sp_DeleteDivision", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@DivisionId", id);

            await conn.OpenAsync();
            var rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }

        // ✅ Get divisions by ClassId
        public async Task<IEnumerable<Division>> GetByClassIdAsync(int classId)
        {
            var divisions = new List<Division>();
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("sp_GetDivisionsByClassId", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ClassId", classId);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                divisions.Add(new Division
                {
                    DivisionId = reader.GetInt32(reader.GetOrdinal("DivisionId")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    ClassId = reader.GetInt32(reader.GetOrdinal("ClassId")),
                    ClassName = reader.IsDBNull(reader.GetOrdinal("ClassName"))
                                ? null
                                : reader.GetString(reader.GetOrdinal("ClassName")),
                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
                });
            }
            return divisions;
        }
    }
        



}


    
  


