using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TestApp.Data.Context;
using TestApp.Domain.Interfaces.Repositories;
using TestApp.Domain.Models.Entities;
using TestApp.Domain.Models.Options;

namespace TestApp.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        public readonly ContextOptions _contextProvider;
        public readonly TestAppContext _dbContext;

        public UserRepository(IOptions<ContextOptions> contextOptions)
        {
            _contextProvider = contextOptions.Value;
            _dbContext = new TestAppContext(_contextProvider.TestAppContext);
        }

        public async Task<User> Select(int id)
        {
            var result = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == id);
            return result;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            var query = from _search in _dbContext.Users select _search;
            var result = await query.AsNoTracking().OrderByDescending(x => x.UserId).ToListAsync();
            return result;
        }

        public async Task<int> Insert(User user)
        {
            _dbContext.Add(user);
            await _dbContext.SaveChangesAsync();
            return user.UserId;
        }
    }
}
