using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace DB_Assigment.Models
{
    public class User:IdentityUser
    {
        [Required]
        public string? FullName { get; set; }
        public ICollection<BorrowingHistory> BorrowingHistories { get; set; }
    }
}
