using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DB_Assigment.DTOs
{
    public class RegisterDto
    {
        [Required]
        [MinLength(3)]
        public string FullName { get; set; }
        [Required]
        [MinLength(3)]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Phone]
        public string Phone { get; set; }
        [Required]
        [PasswordPropertyText]
        public string Password { get; set; }
    }
}
