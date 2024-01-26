using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.Domain.Models.Entities
{
    [Table("ExternalClientMessage")]
    public record ExternalClientMessage
    {
        public ExternalClientMessage()
        {

        }

        public ExternalClientMessage(int userId, int statusCode)
        {
            UserId = userId;
            StatusCode = statusCode;
            CreateDate = DateTime.Now;
        }

        public int ExternalClientMessageId { get; set; }
        public int UserId { get; set; }
        public int StatusCode { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
