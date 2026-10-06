using System.Data;
using Microsoft.Data.SqlClient;
using SchoolApi.Models;
using SchoolApi;
using System.Threading.Tasks;

namespace SchoolApi.Repositories
{
    public interface IUserRepository
    {

        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByIdAsync(int userId);
        Task<int> AddAsync(User user);
        Task<bool> ExistsAsync(string username);
    }

}
