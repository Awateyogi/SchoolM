using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Data;
using SchoolApi.Models;

namespace SchoolApi.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly string _connectionString = "Server=localhost\\SQLEXPRESS;Database=SchoolDb;Trusted_Connection=True;";


        public StudentRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

 
        //public async Task<IEnumerable<Student>> GetAllAsync()
        //{
        //    var students = new List<Student>();

        //    using var conn = new SqlConnection(_connectionString);
        //    using var cmd = new SqlCommand("GetAllStudents", conn);
        //    cmd.CommandType = CommandType.StoredProcedure;

        //    await conn.OpenAsync();
        //    using var reader = await cmd.ExecuteReaderAsync();
        //    while (await reader.ReadAsync())
        //    {
        //        students.Add(new Student
        //        {
        //            StudentId = reader.GetInt32(0),
        //            FirstName = reader.GetString(1),
        //            LastName = reader.GetString(2),
        //            DateOfBirth = reader.IsDBNull(3) ? null : reader.GetDateTime(3),
        //            Gender = reader.IsDBNull(4) ? null : reader.GetString(4),
        //            Email = reader.IsDBNull(5) ? null : reader.GetString(5),
        //            Phone = reader.IsDBNull(6) ? null : reader.GetString(6),
        //            ClassId = reader.IsDBNull(7) ? null : reader.GetInt32(7),
        //            CreatedAt = reader.IsDBNull(8) ? null : reader.GetDateTime(8)
        //        });
        //    }

        //    return students;
        //}


        public async Task<IEnumerable<Student>> GetAllAsync()
        {
            var students = new List<Student>();
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("GetAllStudents", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                students.Add(new Student
                {
                    StudentId = reader.GetInt32(reader.GetOrdinal("StudentId")),
                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                    LastName = reader.GetString(reader.GetOrdinal("LastName")),
                    DateOfBirth = reader.IsDBNull(reader.GetOrdinal("DateOfBirth"))
                        ? (DateTime?)null
                        : reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                    Gender = reader.IsDBNull(reader.GetOrdinal("Gender"))
                        ? null
                        : reader.GetString(reader.GetOrdinal("Gender")),
                    Email = reader.IsDBNull(reader.GetOrdinal("Email"))
                        ? null
                        : reader.GetString(reader.GetOrdinal("Email")),
                    Phone = reader.IsDBNull(reader.GetOrdinal("Phone"))
                        ? null
                        : reader.GetString(reader.GetOrdinal("Phone")),
                    ClassId = reader.IsDBNull(reader.GetOrdinal("ClassId"))
                        ? (int?)null
                        : reader.GetInt32(reader.GetOrdinal("ClassId")),
                    ClassName = reader.IsDBNull(reader.GetOrdinal("ClassName"))
                        ? null
                        : reader.GetString(reader.GetOrdinal("ClassName")),
                    DivisionId = reader.IsDBNull(reader.GetOrdinal("DivisionId"))
                        ? (int?)null
                        : reader.GetInt32(reader.GetOrdinal("DivisionId")),
                    DivisionName = reader.IsDBNull(reader.GetOrdinal("DivisionName"))
                        ? null
                        : reader.GetString(reader.GetOrdinal("DivisionName")),
                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
                });
            }

            return students;
        }


        public async Task<Student?> GetByIdAsync(int id)
        {
            Student student = null;
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("sp_GetStudentById", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@StudentId", id);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                //student =  new Student
                // {
                //     StudentId = reader.GetInt32(reader.GetOrdinal("StudentId")),
                //     FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                //     LastName = reader.GetString(reader.GetOrdinal("LastName")),
                //     DateOfBirth = reader.IsDBNull(reader.GetOrdinal("DateOfBirth"))
                // ? (DateTime?)null
                // : reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                //     Gender = reader.IsDBNull(reader.GetOrdinal("Gender"))
                // ? null
                // : reader.GetString(reader.GetOrdinal("Gender")),
                //     Email = reader.IsDBNull(reader.GetOrdinal("Email"))
                // ? null
                // : reader.GetString(reader.GetOrdinal("Email")),
                //     Phone = reader.IsDBNull(reader.GetOrdinal("Phone"))
                // ? null
                // : reader.GetString(reader.GetOrdinal("Phone")),
                //     ClassId = reader.IsDBNull(reader.GetOrdinal("ClassId"))
                // ? (int?)null
                // : reader.GetInt32(reader.GetOrdinal("ClassId")),
                //     ClassName = reader.IsDBNull(reader.GetOrdinal("ClassName"))
                // ? null
                // : reader.GetString(reader.GetOrdinal("ClassName"))
                // };
                student = new Student
                {
                    StudentId = reader.GetInt32("StudentId"),
                    FirstName = reader.GetString("FirstName"),
                    LastName = reader.GetString("LastName"),
                    DateOfBirth = reader.IsDBNull("DateOfBirth") ? null : reader.GetDateTime("DateOfBirth"),
                    Gender = reader.IsDBNull("Gender") ? null : reader.GetString("Gender"),
                    Email = reader.IsDBNull("Email") ? null : reader.GetString("Email"),
                    Phone = reader.IsDBNull("Phone") ? null : reader.GetString("Phone"),
                    ClassId = reader.IsDBNull("ClassId") ? null : reader.GetInt32("ClassId"),
                    ClassName = reader.IsDBNull("ClassName") ? null : reader.GetString("ClassName"), // add ClassName
                    DivisionId = reader.IsDBNull("DivisionId") ? null : reader.GetInt32("DivisionId"),
                    DivisionName = reader.IsDBNull("DivisionName") ? null : reader.GetString("DivisionName")
                };
            }
            //await conn.OpenAsync();
            //using var reader = await cmd.ExecuteReaderAsync();
            //if (await reader.ReadAsync())
            //{
            //    return new Student
            //    {
            //        StudentId = reader.GetInt32("StudentId"),
            //        FirstName = reader.GetString("FirstName"),
            //        LastName = reader.GetString("LastName"),
            //        DateOfBirth = reader.IsDBNull("DateOfBirth") ? null : reader.GetDateTime("DateOfBirth"),
            //        Gender = reader.IsDBNull("Gender") ? null : reader.GetString("Gender"),
            //        Email = reader.IsDBNull("Email") ? null : reader.GetString("Email"),
            //        Phone = reader.IsDBNull("Phone") ? null : reader.GetString("Phone"),
            //        ClassId = reader.IsDBNull("ClassId") ? null : reader.GetInt32("ClassId")
            //    };
            //}

            return student;
        }

        public async Task<int> AddAsync(Student student)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("AddStudent", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@FirstName", (object)student.FirstName ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@LastName", (object)student.LastName ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@DateOfBirth", (object)student.DateOfBirth ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Gender", (object)student.Gender ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Email", (object)student.Email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Phone", (object)student.Phone ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ClassId", student.ClassId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@DivisionId", student.ClassId ?? (object)DBNull.Value);


            await conn.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }

        public async Task<bool> UpdateAsync(Student student)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("sp_UpdateStudent", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@StudentId", student.StudentId);
            cmd.Parameters.AddWithValue("@FirstName", student.FirstName);
            cmd.Parameters.AddWithValue("@LastName", student.LastName);
            cmd.Parameters.AddWithValue("@DateOfBirth", (object?)student.DateOfBirth ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Gender", (object?)student.Gender ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Email", (object?)student.Email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Phone", (object?)student.Phone ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ClassId", student.ClassId);
            cmd.Parameters.AddWithValue("@DivisionId", student.ClassId);

            await conn.OpenAsync();
            var rows = await cmd.ExecuteNonQueryAsync();

            return rows > 0; // true if updated, false if not found
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("sp_DeleteStudent", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@StudentId", id);

            await conn.OpenAsync();
            var result = await cmd.ExecuteNonQueryAsync();

            // if sp uses SET NOCOUNT ON, result == -1, 
            // so we treat -1 as success too
            return result >= 0;
        }

        public async Task<IEnumerable<Student>> GetByClassAndDivisionAsync(int classId, int divisionId)
        {
            var students = new List<Student>();
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("sp_GetStudentsByClassAndDivision", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ClassId", classId);
            cmd.Parameters.AddWithValue("@DivisionId", divisionId);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                students.Add(new Student
                {
                    StudentId = reader.GetInt32(reader.GetOrdinal("StudentId")),
                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                    LastName = reader.GetString(reader.GetOrdinal("LastName")),
                    DateOfBirth = reader.IsDBNull(reader.GetOrdinal("DateOfBirth"))
                        ? null
                        : reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                    Gender = reader.IsDBNull(reader.GetOrdinal("Gender")) ? null : reader.GetString(reader.GetOrdinal("Gender")),
                    Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email")),
                    Phone = reader.IsDBNull(reader.GetOrdinal("Phone")) ? null : reader.GetString(reader.GetOrdinal("Phone")),
                    ClassId = reader.GetInt32(reader.GetOrdinal("ClassId")),
                    ClassName = reader.GetString(reader.GetOrdinal("ClassName")),
                    DivisionId = reader.GetInt32(reader.GetOrdinal("DivisionId")),
                    DivisionName = reader.GetString(reader.GetOrdinal("DivisionName")),
                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
                });
            }
            return students;
        }

        public async Task<IEnumerable<Student>> GetByClassAsync(int classId)
        {
            var students = new List<Student>();
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("sp_GetStudentsByClass", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ClassId", classId);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                students.Add(new Student
                {
                    StudentId = reader.GetInt32(reader.GetOrdinal("StudentId")),
                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                    LastName = reader.GetString(reader.GetOrdinal("LastName")),
                    Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email")),
                    Phone = reader.IsDBNull(reader.GetOrdinal("Phone")) ? null : reader.GetString(reader.GetOrdinal("Phone")),
                    DateOfBirth = reader.IsDBNull(reader.GetOrdinal("DateOfBirth")) ? null : reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                    Gender = reader.IsDBNull(reader.GetOrdinal("Gender")) ? null : reader.GetString(reader.GetOrdinal("Gender")),
                    ClassId = reader.GetInt32(reader.GetOrdinal("ClassId")),
                    ClassName = reader.GetString(reader.GetOrdinal("ClassName")),
                    DivisionId = reader.GetInt32(reader.GetOrdinal("DivisionId")),
                    DivisionName = reader.GetString(reader.GetOrdinal("DivisionName")),
                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
                });
            }

            return students;
        }

    }
} 
  
  

