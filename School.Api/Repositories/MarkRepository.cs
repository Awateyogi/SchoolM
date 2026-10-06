using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SchoolApi.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace SchoolApi.Repositories
{
    public class MarkRepository : IMarkRepository
    {
       private readonly string _connectionString = "Server=localhost\\SQLEXPRESS;Database=SchoolDb;Trusted_Connection=True;";


        public MarkRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
 
        public async Task<Marks?> GetByIdAsync(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("sp_GetMarkById", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@MarkId", id);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Marks
                {
                    MarkId = reader.GetInt32(reader.GetOrdinal("MarkId")),
                    StudentId = reader.GetInt32(reader.GetOrdinal("StudentId")),
                    Subject = reader.GetString(reader.GetOrdinal("Subject")),
                    Test1 = reader.GetInt32(reader.GetOrdinal("Test1")),
                    Test2 = reader.GetInt32(reader.GetOrdinal("Test2")),
                    Test3 = reader.GetInt32(reader.GetOrdinal("Test3")),
                    Test4 = reader.GetInt32(reader.GetOrdinal("Test4")),
                    Sem1 = reader.GetInt32(reader.GetOrdinal("Sem1")),
                    Sem2 = reader.GetInt32(reader.GetOrdinal("Sem2"))
                };
            }
            return null;
        }

        public async Task<int> AddAsync(Marks mark)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand("sp_AddMark", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@StudentId", mark.StudentId);
                cmd.Parameters.AddWithValue("@SubjectId", mark.SubjectId);
                cmd.Parameters.AddWithValue("@Test1", mark.Test1 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Test2", mark.Test2 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Test3", mark.Test3 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Test4", mark.Test4 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Sem1", mark.Sem1 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Sem2", mark.Sem2 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Subject", mark.Subject ?? "");

                await conn.OpenAsync();
                var result = await cmd.ExecuteScalarAsync();
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                // log the exception to see why insert failed
                Console.WriteLine("Error inserting mark: " + ex.Message);
                throw;
            }
        }

        public async Task<bool> UpdateAsync(Marks mark)   // ✅ Implemented here
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("sp_UpdateMark", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@MarkId", mark.MarkId);
            cmd.Parameters.AddWithValue("@StudentId", mark.StudentId);
            cmd.Parameters.AddWithValue("@SubjectId", mark.SubjectId);
            cmd.Parameters.AddWithValue("@Test1", mark.Test1);
            cmd.Parameters.AddWithValue("@Test2", mark.Test2);
            cmd.Parameters.AddWithValue("@Test3", mark.Test3);
            cmd.Parameters.AddWithValue("@Test4", mark.Test4);
            cmd.Parameters.AddWithValue("@Sem1", mark.Sem1);
            cmd.Parameters.AddWithValue("@Sem2", mark.Sem2);

            await conn.OpenAsync();
            var rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("sp_DeleteMark", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@MarkId", id);

            await conn.OpenAsync();
            var rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }

        public async Task<IEnumerable<Marks>> GetByStudentIdAsync(int studentId)
        {
            var marks = new List<Marks>();
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("sp_GetMarksByStudentId", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@StudentId", studentId);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                marks.Add(new Marks
                {
                    MarkId = reader.GetInt32(reader.GetOrdinal("MarkId")),
                    StudentId = reader.GetInt32(reader.GetOrdinal("StudentId")),
                    SubjectId = reader.GetInt32(reader.GetOrdinal("SubjectId")),
                    Subject = reader.GetString(reader.GetOrdinal("SubjectName")),
                    //  Subject = reader.IsDBNull(reader.GetOrdinal("Subject"))
                    //? string.Empty
                    //: reader.GetString(reader.GetOrdinal("Subject")),
                    Test1 = reader.IsDBNull(reader.GetOrdinal("Test1")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("Test1")),
                    Test2 = reader.IsDBNull(reader.GetOrdinal("Test2")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("Test2")),
                    Test3 = reader.IsDBNull(reader.GetOrdinal("Test3")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("Test3")),
                    Test4 = reader.IsDBNull(reader.GetOrdinal("Test4")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("Test4")),

                    Sem1 = reader.IsDBNull(reader.GetOrdinal("Sem1")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("Sem1")),
                    Sem2 = reader.IsDBNull(reader.GetOrdinal("Sem2")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("Sem2")),

                    CreatedAt = reader.IsDBNull(reader.GetOrdinal("CreatedAt"))
                        ? DateTime.MinValue
                        : reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
                });
            }
            return marks;
        }

        public async Task<IEnumerable<Marks>> GetByStudentAsync(int studentId)
        {
            var marks = new List<Marks>();

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("sp_GetMarksByStudentId", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@StudentId", studentId);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                marks.Add(new Marks
                {
                    MarkId = reader.GetInt32(reader.GetOrdinal("MarkId")),
                    StudentId = reader.GetInt32(reader.GetOrdinal("StudentId")),
                    Subject = reader.GetString(reader.GetOrdinal("Subject")),
                    Test1 = reader.IsDBNull(reader.GetOrdinal("Test1")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("Test1")),
                    Test2 = reader.IsDBNull(reader.GetOrdinal("Test2")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("Test2")),
                    Test3 = reader.IsDBNull(reader.GetOrdinal("Test3")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("Test3")),
                    Test4 = reader.IsDBNull(reader.GetOrdinal("Test4")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("Test4")),

                    Sem1 = reader.IsDBNull(reader.GetOrdinal("Sem1")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("Sem1")),
                    Sem2 = reader.IsDBNull(reader.GetOrdinal("Sem2")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("Sem2")),

                    CreatedAt = reader.IsDBNull(reader.GetOrdinal("CreatedAt"))
                        ? DateTime.MinValue
                        : reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
                });
            }

            return marks;
        }
    }
}
    

