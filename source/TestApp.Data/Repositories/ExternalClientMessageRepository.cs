using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TestApp.Data.Context;
using TestApp.Domain.Interfaces.Repositories;
using TestApp.Domain.Models.Entities;
using TestApp.Domain.Models.Options;

namespace TestApp.Data.Repositories
{
    public class ExternalClientMessageRepository : IExternalClientMessageRepository
    {
        public readonly ContextOptions _contextProvider;
        public readonly TestAppContext _dbContext;

        public ExternalClientMessageRepository(IOptions<ContextOptions> contextOptions)
        {
            _contextProvider = contextOptions.Value;
            _dbContext = new TestAppContext(_contextProvider.TestAppContext);
        }

        public async Task<int> Insert(ExternalClientMessage externalClientMessage)
        {
            _dbContext.Add(externalClientMessage);
            await _dbContext.SaveChangesAsync();
            return externalClientMessage.ExternalClientMessageId;
        }
    }
}
