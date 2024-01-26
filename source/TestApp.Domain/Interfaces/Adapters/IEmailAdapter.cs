using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.Domain.Interfaces.Adapters
{
    public interface IEmailAdapter
    {
        Task SendEmail(string from, string to, string subject, string body);
    }
}
