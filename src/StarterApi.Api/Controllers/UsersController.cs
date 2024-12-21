using Microsoft.AspNetCore.Mvc;
using StarterApi.Application.Modules.Users.Interfaces;

using StarterApi.Application.Modules.Users.Services;
using StarterApi.Application.Common.Exceptions;
using StarterApi.Application.Modules.Users.DTOs;

namespace StarterApi.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ITenantUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(
            ITenantUserService userService,
            ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser(CreateUserDto dto)
        {
            try
            {
                var user = await _userService.CreateUserAsync(dto);
                return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user");
                return StatusCode(500, "An error occurred while creating the user");
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            try
            {
                var users = await _userService.GetUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving users");
                return StatusCode(500, "An error occurred while retrieving users");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(Guid id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                return Ok(user);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user");
                return StatusCode(500, "An error occurred while retrieving the user");
            }
        }
    }
} 