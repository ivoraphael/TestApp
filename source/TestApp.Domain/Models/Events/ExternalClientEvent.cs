using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApp.Domain.Models.Entities;

namespace TestApp.Domain.Models.Events
{
    public record ExternalClientEvent
    {
        public ExternalClientEvent(int type, User user)
        {
            Type = type;
            this.User = user;
        }

        public int Type { get; set; }

        public User User { get; set; }
    }
}
