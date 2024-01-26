using FluentValidation.Results;
using Newtonsoft.Json;
using TestApp.Application.Validations;
using TestApp.Domain.Adapters;
using TestApp.Domain.Interfaces.Repositories;
using TestApp.Domain.Models.Entities;
using TestApp.Domain.Models.Events;
using TestApp.Domain.Services;


namespace TestApp.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IPublishAdapter _publishAdapter;
        private readonly IUserRepository _userRepository;

        public UserService(IPublishAdapter publishAdapter, IUserRepository userRepository) =>
            (_publishAdapter, _userRepository) = (publishAdapter, userRepository);

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            var users = await _userRepository.GetAll();
            return users;
        }

        public async Task<int> SaveUser(User user)
        {
            var userValidation = ValidateUser(user);

            if (!userValidation.Key)
            {
                throw new Exception(userValidation.Value);
            }

            var userId = await _userRepository.Insert(user);
            user = await _userRepository.Select(userId);
            await IntegrateWithExternalClient(user, userId);

            return userId;
        }

        public KeyValuePair<bool,string> ValidateUser(User user)
        {
            var userValidation = new UserValidation().Validate(user);

            if (!userValidation.IsValid)
            {
                var message = userValidation.Errors.Select(m => m.ErrorMessage).FirstOrDefault();
                return new KeyValuePair<bool, string>(false, message);
            }

            return new KeyValuePair<bool, string>(true, string.Empty);
        }

        private async Task IntegrateWithExternalClient(User user, int userId)
        {
            var externalClientEvent = new ExternalClientEvent(1, user);
            await _publishAdapter.SendMessage(JsonConvert.SerializeObject(externalClientEvent));
        }
    }
}
