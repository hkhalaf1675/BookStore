using DB_Assigment.DTOs;

namespace DB_Assigment.IRepositories
{
    public interface IBorrowingRespository
    {
        Task<ICollection<UserBorrowedBookDto>> GetUserBorrowedBooksAsync(string userId);
        Task<LogicResponse> BorrowBookAync(string userId,string bookCode);
        Task<LogicResponse> UnBorrowBookAsync(string userId,string bookCode);
        Task<ICollection<BookBorrowingHistory>> GetBookBorrowingHistoryAsync(string bookCode);
    }
}
