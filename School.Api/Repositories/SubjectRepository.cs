using System;
using System.Data;
using Microsoft.Data.SqlClient;
using SchoolApi.Models;
using SchoolApi;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SchoolApi.Repositories
{
    public class SubjectRepository : ISubjectRepository
    {
        private readonly string _connectionString = "Server=localhost\\SQLEXPRESS;Database=SchoolDb;Trusted_Connection=True;";
       
        public SubjectRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        //public async Task<IEnumerable<Subject>> GetAllAsync()
        //{
        //    var subjects = new List<Subject>();

        //    using (var conn = new SqlConnection(_connectionString))
        //    {
        //        await conn.OpenAsync();
        //        var cmd = new SqlCommand("SELECT SubjectId, Name FROM Subjects", conn);

        //        using (var reader = await cmd.ExecuteReaderAsync())
        //        {
        //            while (await reader.ReadAsync())
        //            {
        //                subjects.Add(new Subject
        //                {
        //                    SubjectId = reader.GetInt32(0),
        //                    Name = reader.GetString(1)
        //                });
        //            }
        //        }
        //    }

        //    return subjects;
        //}
        //public async Task<IEnumerable<Marks>> GetByStudentAsync(int studentId)
        //{
        //    using (var connection = new SqlConnection(_connectionString))
        //    {
        //        var results = await connection.QueryAsync<Marks>(
        //            "GetMarksByStudent",
        //            new { StudentId = studentId },
        //            commandType: CommandType.StoredProcedure
        //        );
        //        return results.ToList();
        //    }
        //}
        public async Task<IEnumerable<Subject>> GetAllAsync()
        {
            var subjects = new List<Subject>();

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_GetAllSubjects", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                await conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        subjects.Add(new Subject
                        {
                            SubjectId = reader.GetInt32(0),
                            Name = reader.GetString(1)
                        });
                    }
                }
            }

            return subjects;
        }

        public async Task<Subject> GetByIdAsync(int id)
        {
            Subject subject = null;

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_GetSubjectById", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SubjectId", id);
                await conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        subject = new Subject
                        {
                            SubjectId = reader.GetInt32(0),
                            Name = reader.GetString(1)
                        };
                    }
                }
            }

            return subject;
        }

        public async Task AddAsync(Subject subject)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_AddSubject", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Name", subject.Name);
                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task UpdateAsync(Subject subject)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_UpdateSubject", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SubjectId", subject.SubjectId);
                cmd.Parameters.AddWithValue("@Name", subject.Name);
                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_DeleteSubject", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SubjectId", id);
                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
        }

    }
}


