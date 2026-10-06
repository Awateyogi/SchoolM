using System.Collections.Generic;
using System.Threading.Tasks;
using SchoolApi.Models;

namespace SchoolApi.Repositories
{
	public interface IMarkRepository
	{
		
		Task<int> AddAsync(Marks mark);
		Task<bool> DeleteAsync(int markId);
        Task<bool> UpdateAsync(Marks mark);   // 
        Task<IEnumerable<Marks>> GetByStudentAsync(int studentId);
		Task<IEnumerable<Marks>> GetByStudentIdAsync(int studentId);

    }
}
