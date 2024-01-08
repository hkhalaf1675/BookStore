using DB_Assigment.DTOs;

namespace DB_Assigment.IRepository
{
    public interface IAuthRepository
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
    }
}
