using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApp.Domain.Models.Events;

namespace TestApp.Domain.Interfaces.Adapters
{
    public interface IExternalClientAdapter
    {
        Task ProcessMessage(ExternalClientEvent externalClientEvent, string queue);
    }
}
