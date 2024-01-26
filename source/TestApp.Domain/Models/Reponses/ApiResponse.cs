using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.Domain.Models.Reponses
{
    internal record ApiResponse
    {
        public string Message { get; set; }
        public object Result { get; set; }
        public List<string> Errors { get; set; }
    }
}
