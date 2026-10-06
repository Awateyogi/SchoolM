using System;
using SchoolApi.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SchoolApi.Repositories
{

    public interface ISubjectRepository
    {
        Task<IEnumerable<Subject>> GetAllAsync();
        Task<Subject?> GetByIdAsync(int id);
       
      

        Task AddAsync(Subject subject);
        Task UpdateAsync(Subject subject);
        Task DeleteAsync(int id);
    }
} 
    /// <summary>
    /// Summary description for Class1
  
