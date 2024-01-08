using DB_Assigment.DTOs;
using DB_Assigment.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DB_Assigment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAuthRepository authRepository;

        public AccountsController(IAuthRepository _authRepository)
        {
            authRepository = _authRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            AuthResponseDto authDto = await authRepository.RegisterAsync(registerDto);

            if(authDto.Message != null)
            {
                return BadRequest($"{authDto.Message}");
            }

            return Ok(authDto.Token);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            AuthResponseDto authDto = await authRepository.LoginAsync(loginDto);

            if(authDto.Message != null)
            {
                return BadRequest($"{authDto.Message}");
            }

            return Ok(authDto.Token);
        }
    }
}
