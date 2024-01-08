using System.Text.Json.Serialization;

namespace DB_Assigment.DTOs
{
    public class AuthResponseDto
    {
        public string? Message { get; set; }
        public string? Token { get; set; }
    }
}
