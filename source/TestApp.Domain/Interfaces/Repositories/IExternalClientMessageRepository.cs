using TestApp.Domain.Models.Entities;

namespace TestApp.Domain.Interfaces.Repositories
{
    public interface IExternalClientMessageRepository
    {
        Task<int> Insert(ExternalClientMessage externalClientMessage);
    }
}
