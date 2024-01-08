namespace DB_Assigment.DTOs
{
    public class BookBorrowingHistory
    {
        public string? Code { get; set; }
        public string? Title { get; set; }
        public string? UserName { get; set; }
        public DateTime? BorrowedDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}
