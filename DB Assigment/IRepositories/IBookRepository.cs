using DB_Assigment.DTOs;

namespace DB_Assigment.IRepositories
{
    public interface IBookRepository
    {
        Task<BookDto?> GetAsync(string bookCode);
        Task<LogicResponse> AddNewAsync(BookDto newBookDto);
        Task<LogicResponse> UpdateAsync(BookDto bookDto);
        Task<LogicResponse> DeleteAsync(string bookCode);
    }
}
