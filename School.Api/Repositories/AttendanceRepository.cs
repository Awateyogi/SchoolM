using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using School.Api.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace SchoolApi.Repositories
{
    public class AttendanceRepository : IAttendanceRepository
    {
        private readonly string _connectionString;
    

     public AttendanceRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        } 

        public async Task<int> AddAsync(Attendance attendance)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("sp_AddAttendance", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@StudentId", attendance.StudentId);
            cmd.Parameters.AddWithValue("@ClassId", attendance.ClassId);
            cmd.Parameters.AddWithValue("@DivisionId", (object?)attendance.DivisionId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Date", attendance.Date.Date);
            cmd.Parameters.AddWithValue("@Status", attendance.Status ?? "");
            cmd.Parameters.AddWithValue("@Notes", (object?)attendance.Notes ?? DBNull.Value);

            await conn.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }

        public async Task<IEnumerable<Attendance>> GetByStudentAsync(int studentId)
        {
            var list = new List<Attendance>();
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("sp_GetAttendanceByStudent", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@StudentId", studentId);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new Attendance
                {
                    AttendanceId = reader.GetInt32(reader.GetOrdinal("AttendanceId")),
                    StudentId = reader.GetInt32(reader.GetOrdinal("StudentId")),
                    ClassId = reader.GetInt32(reader.GetOrdinal("ClassId")),
                    DivisionId = reader.IsDBNull(reader.GetOrdinal("DivisionId")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("DivisionId")),
                    Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                    Status = reader.GetString(reader.GetOrdinal("Status")),
                    Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? null : reader.GetString(reader.GetOrdinal("Notes")),
                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
                });
            }

            return list;
        }

        public async Task<IEnumerable<Attendance>> GetByClassAndDateAsync(int classId, DateTime? date = null)
        {
            var list = new List<Attendance>();
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("sp_GetAttendanceByClassAndDate", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@ClassId", classId);
            cmd.Parameters.AddWithValue("@Date", (object?)date?.Date ?? DBNull.Value);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new Attendance
                {
                    AttendanceId = reader.GetInt32(reader.GetOrdinal("AttendanceId")),
                    StudentId = reader.GetInt32(reader.GetOrdinal("StudentId")),
                    ClassId = reader.GetInt32(reader.GetOrdinal("ClassId")),
                    DivisionId = reader.IsDBNull(reader.GetOrdinal("DivisionId")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("DivisionId")),
                    Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                    Status = reader.GetString(reader.GetOrdinal("Status")),
                    Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? null : reader.GetString(reader.GetOrdinal("Notes")),
                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
                });
            }

            return list;
        }

        public async Task<bool> UpdateAsync(Attendance attendance)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("sp_UpdateAttendance", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@AttendanceId", attendance.AttendanceId);
            cmd.Parameters.AddWithValue("@StudentId", attendance.StudentId);
            cmd.Parameters.AddWithValue("@ClassId", attendance.ClassId);
            cmd.Parameters.AddWithValue("@DivisionId", (object?)attendance.DivisionId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Date", attendance.Date.Date);
            cmd.Parameters.AddWithValue("@Status", attendance.Status ?? "");
            cmd.Parameters.AddWithValue("@Notes", (object?)attendance.Notes ?? DBNull.Value);

            await conn.OpenAsync();
            var rows = await cmd.ExecuteScalarAsync(); // sp returns @@ROWCOUNT
            if (rows == null) return false;
            return Convert.ToInt32(rows) > 0;
        }

        public async Task<bool> DeleteAsync(int attendanceId)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("sp_DeleteAttendance", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@AttendanceId", attendanceId);

            await conn.OpenAsync();
            var rows = await cmd.ExecuteScalarAsync();
            if (rows == null) return false;
            return Convert.ToInt32(rows) > 0;
        }
    }
}

