using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.Domain.Adapters
{
    public interface IPublishAdapter
    {
        Task SendMessage(string stringfiedMessage, string? queue = null);
    }
}
