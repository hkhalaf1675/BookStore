using System.ComponentModel.DataAnnotations;

namespace DB_Assigment.DTOs
{
    public class BookDto
    {
        [Required]
        [MinLength(3)]
        public string Code { get; set; }
        [Required]
        [MinLength(3)]
        public string Title { get; set; }
        public string? Category { get; set; }
        public string? Author { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        public int Count { get; set; }
    }
}
