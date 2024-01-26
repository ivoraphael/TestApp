using Bogus;
using FluentAssertions;
using Microsoft.Extensions.Hosting;
using Moq;
using System.Globalization;
using TestApp.Application.Services;
using TestApp.Domain.Adapters;
using TestApp.Domain.Interfaces.Repositories;
using TestApp.Domain.Models.Entities;

namespace TestApp.Tests.Services
{
    [TestClass]
    public class UserServiceTest
    {
        private readonly Mock<IUserRepository> _userRepositoryMoq;
        private readonly Mock<IPublishAdapter> _publishAdapterMoq;
        private readonly Mock<IHostEnvironment> _envMoq;

        private readonly UserService _userService;

        public UserServiceTest()
        {
            _userRepositoryMoq = new Mock<IUserRepository>();
            _publishAdapterMoq = new Mock<IPublishAdapter>();
            _envMoq = new Mock<IHostEnvironment>();

            _userService = new UserService(_publishAdapterMoq.Object,
                _userRepositoryMoq.Object);
        }

        [TestMethod]
        public void IdadeInvalidaDeveReprovarValidacao()
        {
            var user = new User("John Doe", 0);

            var result = _userService.ValidateUser(user);

            result.Key.Should().Be(false);
            result.Value.Should().Be("Idade é obrigatória.");
        }

        [TestMethod]
        public void NomeInvalidoDeveReprovarValidacao()
        {
            var user = new User("", 32);

            var result = _userService.ValidateUser(user);

            result.Key.Should().Be(false);
            result.Value.Should().Be("Nome é obrigatório.");
        }
    }
}