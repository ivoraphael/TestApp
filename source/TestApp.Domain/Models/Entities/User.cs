using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.Domain.Models.Entities
{
    [Table("User")]
    public record User
    {
        public User()
        {

        }

        public User(string name, int age)
        {
            Name = name;
            Age = age;
        }

        public int UserId { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
