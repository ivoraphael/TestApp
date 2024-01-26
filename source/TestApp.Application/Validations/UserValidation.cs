using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApp.Domain.Models.Entities;

namespace TestApp.Application.Validations
{
    internal class UserValidation : AbstractValidator<User>
    {
        public UserValidation()
        {
            RuleFor(v => v.Name)
                .NotEmpty()
                .WithMessage("Nome é obrigatório.");

            RuleFor(v => v.Age)
                    .GreaterThan(0)
                    .WithMessage("Idade é obrigatória.");
        }
    }
}
