using Microsoft.Extensions.Configuration;
using SchoolApi.Models;
using SchoolApi.Repositories;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;

public class FeeRepository : IFeeRepository
{
    private readonly string _connectionString;

    public FeeRepository(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("DefaultConnection");
    }

    public async Task<int> AddAsync(Fee fee)
    {
        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand("sp_AddFee", conn);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@StudentId", fee.StudentId);
        cmd.Parameters.AddWithValue("@ClassId",fee.ClassId);
        cmd.Parameters.AddWithValue("@Amount", fee.Amount);
        cmd.Parameters.AddWithValue("@PaidAmount", fee.PaidAmount);
        cmd.Parameters.AddWithValue("@PaidAt", fee.PaidAt);

        await conn.OpenAsync();
        var result = await cmd.ExecuteScalarAsync();
        return Convert.ToInt32(result);
    }

    public async Task UpdateAsync(Fee fee)
    {
        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand("sp_UpdateFee", conn);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@FeeId", fee.FeeId);
        cmd.Parameters.AddWithValue("@PaidAmount", fee.PaidAmount);

        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task<IEnumerable<Fee>> GetByStudentIdAsync(int studentId)
    {
        var fees = new List<Fee>();
        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand("sp_GetFeesByStudent", conn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@StudentId", studentId);

        await conn.OpenAsync();
        using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            fees.Add(new Fee
            {
                FeeId = reader.GetInt32(reader.GetOrdinal("FeeId")),
                StudentId = reader.GetInt32(reader.GetOrdinal("StudentId")),
                ClassId = reader.GetInt32(reader.GetOrdinal("ClassId")),
                Amount = reader.GetDecimal(reader.GetOrdinal("Amount")),
                PaidAmount = reader.GetDecimal(reader.GetOrdinal("PaidAmount")),
              //  Balance = reader.GetDecimal(reader.GetOrdinal("Balance")),
                //DueDate = reader.GetDateTime(reader.GetOrdinal("DueDate")),
                PaidAt = reader.GetDateTime(reader.GetOrdinal("PaidAt"))
            });
        }
        return fees;
    }

    public async Task<IEnumerable<Fee>> GetByStudentAndClassAsync(int studentId, int classId)
    {
        var fees = new List<Fee>();
        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand("sp_GetFeesByStudentAndClass", conn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@StudentId", studentId);
        cmd.Parameters.AddWithValue("@ClassId", classId);

        await conn.OpenAsync();
        using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            fees.Add(new Fee
            {
                FeeId = reader.GetInt32(reader.GetOrdinal("FeeId")),
                StudentId = reader.GetInt32(reader.GetOrdinal("StudentId")),
                ClassId = reader.GetInt32(reader.GetOrdinal("ClassId")),
                Amount = reader.GetDecimal(reader.GetOrdinal("Amount")),
                PaidAmount = reader.GetDecimal(reader.GetOrdinal("PaidAmount")),
             //   Balance = reader.GetDecimal(reader.GetOrdinal("Balance")),
                // DueDate = reader.GetDateTime(reader.GetOrdinal("DueDate")),
                PaidAt = reader.GetDateTime(reader.GetOrdinal("PaidAt"))
            });
        }
        return fees;
    }

}
