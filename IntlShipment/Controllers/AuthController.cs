using IntlShipment.DTOs;
using IntlShipment.Helpers;
using IntlShipment.Models;
using IntlShipment.Services;
using Microsoft.AspNetCore.Mvc;

namespace IntlShipment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<Response<string>>> Login([FromBody] LoginDTO request)
        {
            var response = await _authService.Login(request.Email, request.Password);
            if (response.Success)
                return Ok(response);
            else
                return Unauthorized(response);
        }

        [HttpPost("register")]
        public async Task<ActionResult<Response<User>>> Register([FromBody] User user)
        {
            var response = await _authService.Register(user);
            if (response.Success)
                return Ok(response);
            else
                return BadRequest(response);
        }
    }
}
