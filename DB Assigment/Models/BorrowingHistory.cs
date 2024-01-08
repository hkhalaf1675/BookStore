using System.ComponentModel.DataAnnotations.Schema;

namespace DB_Assigment.Models
{
    public class BorrowingHistory
    {
        public int Id { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        [ForeignKey("Book")]
        public string BookCode { get; set; }
        public User User { get; set; }
        public Book Book { get; set; }
    }
}
