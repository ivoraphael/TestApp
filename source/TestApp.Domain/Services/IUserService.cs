using TestApp.Domain.Models.Entities;

namespace TestApp.Domain.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsers();

        Task<int> SaveUser(User user);
    }
}
