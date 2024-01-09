using DB_Assigment.DTOs;
using DB_Assigment.IRepository;
using DB_Assigment.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;

namespace DB_Assigment.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration configuration;

        public AuthRepository(UserManager<User> _userManager,RoleManager<IdentityRole> _roleManager,IConfiguration _configuration)
        {
            userManager = _userManager;
            roleManager = _roleManager;
            configuration = _configuration;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            if(await userManager.FindByEmailAsync(registerDto.Email) != null)
            {
                return new AuthResponseDto
                {
                    Message = "That Email or Username is already exists"
                };
            }

            if(await userManager.FindByNameAsync(registerDto.UserName) != null)
            {
                return new AuthResponseDto
                {
                    Message = "That Email or Username is already exists"
                };
            }

            User user = new User
            {
                FullName = registerDto.FullName,
                UserName = registerDto.UserName,
                Email = registerDto.Email,
            };

            var result = await userManager.CreateAsync(user, registerDto.Password);

            if(!result.Succeeded)
            {
                string errors = "";
                foreach (var error in result.Errors)
                {
                    errors += $"{error.Description}";
                }
                return new AuthResponseDto
                {
                    Message = errors
                };
            }

            // check if the role exists or not
            bool checkRoleExists = await EnsureRoleExistsAsync("Client");
            if (!checkRoleExists)
            {
                return new AuthResponseDto
                {
                    Message = "Error on create Role"
                };
            }
            
            // add the role to the new user
            await userManager.AddToRoleAsync(user, "Client");

            // call the method that create the token
            var token = await CreateJwtTokenAsync(user);

            if (token == null)
            {
                return new AuthResponseDto
                {
                    Message = "Error on Jwt Token Creation"
                };
            }

            return new AuthResponseDto
            {
                Token = token
            };
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            User? user = await userManager.FindByEmailAsync(loginDto.Email);
            if(user == null)
            {
                return new AuthResponseDto
                {
                    Message = "The email or password is not correct"
                };

            }

            bool found = await userManager.CheckPasswordAsync(user, loginDto.Password);

            if (!found)
            {
                return new AuthResponseDto
                {
                    Message = "the email or password is not correct"
                };
            }

            // call the method that create token
            var token = await CreateJwtTokenAsync(user);

            if(token is null)
            {
                return new AuthResponseDto
                {
                    Message = "error on jwt token creation"
                };
            }

            return new AuthResponseDto
            {
                Token = token
            };
        }

        private async Task<string?> CreateJwtTokenAsync(User user)
        {
            // get and add the claims
            List<Claim> claims = new List<Claim>();

            claims.Add(new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()));
            claims.Add(new Claim("UserName",user.UserName));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));

            var roles = await userManager.GetRolesAsync(user);
            foreach(var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role,role.ToString()));
            }

            // create the security key
            SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("JWT:Key")));

            // create the signincredentials
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // create the jwt token
            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken
                (
                    claims: claims,
                    signingCredentials: signingCredentials,
                    expires: DateTime.Now.AddMinutes(configuration.GetValue<double>("JWT:DurationInMinutes"))
                );

            // return the token in format string
            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }

        private async Task<bool> EnsureRoleExistsAsync(string role)
        {
            if (await roleManager.RoleExistsAsync(role) == false)
            {
                var roleCheck = await roleManager.CreateAsync(new IdentityRole
                {
                    Name = role
                });

                if (!roleCheck.Succeeded)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
