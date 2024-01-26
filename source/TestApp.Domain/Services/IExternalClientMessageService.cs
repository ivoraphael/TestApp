using TestApp.Domain.Models.Entities;

namespace TestApp.Domain.Services
{
    public interface IExternalClientMessageService
    {
        Task<int> SaveExternalClientMessage(ExternalClientMessage externalClientMessage);
    }
}
