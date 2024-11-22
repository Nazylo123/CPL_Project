using Microsoft.AspNetCore.Mvc;
using ShoesStore.ViewModel.RequestModel;
using ShoesStore.IRepository;
using System.ComponentModel.DataAnnotations;

namespace ShoesStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userRepository.GetAllUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/Users/{email}
        [HttpGet("{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email) || !new EmailAddressAttribute().IsValid(email))
            {
                return BadRequest("Invalid email format.");
            }

            try
            {
                var user = await _userRepository.GetUserByEmailAsync(email);
                if (user == null)
                {
                    return NotFound($"User with email '{email}' not found.");
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/Users/{email}
        [HttpPut("{email}")]
        public async Task<IActionResult> UpdateUser(string email, [FromBody] UserRequestViewModel request)
        {
            if (string.IsNullOrWhiteSpace(email) || !new EmailAddressAttribute().IsValid(email))
            {
                return BadRequest("Invalid email format.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updatedUser = await _userRepository.UpdateUserAsync(email, request);
                if (updatedUser == null)
                {
                    return NotFound($"User with email '{email}' not found.");
                }

                return Ok(updatedUser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/Users/{email}
        [HttpDelete("{email}")]
        public async Task<IActionResult> DeleteUser(string email)
        {
            if (string.IsNullOrWhiteSpace(email) || !new EmailAddressAttribute().IsValid(email))
            {
                return BadRequest("Invalid email format.");
            }

            try
            {
                var isDeleted = await _userRepository.DeleteUserAsync(email);
                if (!isDeleted)
                {
                    return NotFound($"User with email '{email}' not found.");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
