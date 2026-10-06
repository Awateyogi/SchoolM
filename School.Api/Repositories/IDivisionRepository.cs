using System.Collections.Generic;
using System.Threading.Tasks;
using SchoolApi.Models;

namespace SchoolApi.Repositories
{

    public interface IDivisionRepository
    {
        Task<IEnumerable<Division>> GetAllAsync();
        Task<Division?> GetByIdAsync(int id);   // only one GetByIdAsync
        Task<int> AddAsync(Division division);
        Task<bool> UpdateAsync(Division division);
        Task<bool> DeleteAsync(int id);

        // fetch all divisions by class
        Task<IEnumerable<Division>> GetByClassIdAsync(int classId);
    }
} 
   
