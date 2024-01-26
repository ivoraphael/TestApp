using Microsoft.AspNetCore.Mvc;
using TestApp.Domain.Models.Entities;
using TestApp.Domain.Services;

namespace TestApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _UserService;

        public UserController(IUserService UserService) =>
            _UserService = UserService;

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var Users = await _UserService.GetAllUsers();

                return Ok(Users);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add(User User)
        {
            try
            {
                var id = await _UserService.SaveUser(User);

                return Ok(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}