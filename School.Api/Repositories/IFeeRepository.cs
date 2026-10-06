using SchoolApi.Models;

namespace SchoolApi.Repositories
{
    public interface IFeeRepository
    {
        Task<IEnumerable<Fee>> GetByStudentAndClassAsync(int studentId, int classId);

        Task<IEnumerable<Fee>> GetByStudentIdAsync(int studentId);
        Task<int> AddAsync(Fee fee);
        Task UpdateAsync(Fee fee);
       // Task DeleteAsync(int feeId);
    }
}
