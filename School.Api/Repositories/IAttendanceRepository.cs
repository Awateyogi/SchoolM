using School.Api.Models;
using SchoolApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace SchoolApi.Repositories
{
    public interface IAttendanceRepository
    {
        Task<int> AddAsync(Attendance attendance);
        Task<IEnumerable<Attendance>> GetByStudentAsync(int studentId);
        Task<IEnumerable<Attendance>> GetByClassAndDateAsync(int classId, DateTime? date = null);
        Task<bool> UpdateAsync(Attendance attendance);
        Task<bool> DeleteAsync(int attendanceId);
    }
}
