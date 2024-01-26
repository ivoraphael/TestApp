using TestApp.Domain.Models.Entities;

namespace TestApp.Domain.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User> Select(int id);
        Task<IEnumerable<User>> GetAll();
        Task<int> Insert(User club);
    }
}
