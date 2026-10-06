using SchoolApi.Models;

namespace SchoolApi.Repositories
{
    public interface IStudentRepository
    {
        Task<IEnumerable<Student>> GetAllAsync();
        Task<Student?> GetByIdAsync(int id);
        Task<int> AddAsync(Student student);
        Task<bool> UpdateAsync(Student student);
        Task<bool> DeleteAsync(int id);

        Task<IEnumerable<Student>> GetByClassAsync(int classId);
      
        Task<IEnumerable<Student>> GetByClassAndDivisionAsync(int classId, int divisionId);
    }
}
