using Newtonsoft.Json;
using TestApp.Domain.Adapters;
using TestApp.Domain.Interfaces.Repositories;
using TestApp.Domain.Models.Entities;
using TestApp.Domain.Models.Events;
using TestApp.Domain.Services;


namespace TestApp.Application.Services
{
    public class ExternalClientMessageService : IExternalClientMessageService
    {
        private readonly IExternalClientMessageRepository _externalClientMessageRepository;

        public ExternalClientMessageService(IExternalClientMessageRepository ExternalClientMessageRepository) =>
            (_externalClientMessageRepository) = (ExternalClientMessageRepository);

        public async Task<int> SaveExternalClientMessage(ExternalClientMessage externalClientMessage)
        {
            try
            {
                var externalClientMessageId = await _externalClientMessageRepository.Insert(externalClientMessage);

                if (externalClientMessageId > 0)
                {
                    return externalClientMessageId;
                }

                return 0;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
