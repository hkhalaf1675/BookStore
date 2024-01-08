using DB_Assigment.Contexts;
using DB_Assigment.DTOs;
using DB_Assigment.IRepositories;
using DB_Assigment.Models;
using DB_Assigment.Profiles;
using System.Net.WebSockets;

namespace DB_Assigment.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly ApplicationDbContext context;

        public BookRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<BookDto?> GetAsync(string bookCode)
        {
            var book = context.Books.FirstOrDefault(B => B.Code == bookCode);

            if(book == null)
            {
                return null;
            }

            return MappingBook.MapBookToDto(book);
        }
        public async Task<LogicResponse> AddNewAsync(BookDto newBookDto)
        {
            LogicResponse logicResponse = new();

            Book newBook = MappingBook.MapDtoToBook(newBookDto);
            context.Books.Add(newBook);

            try
            {
                context.SaveChanges();

                logicResponse.IsJobDone = true;
                return logicResponse;
            }
            catch(Exception ex)
            {
                logicResponse.Message = ex.Message;
                return logicResponse;
            }
        }

        public async Task<LogicResponse> DeleteAsync(string bookCode)
        {
            LogicResponse logicResponse = new();

            var targetBook = context.Books.FirstOrDefault(B => B.Code == bookCode);

            if(targetBook == null)
            {
                logicResponse.Message = "There is no Book with that Code";
                return logicResponse;
            }

            context.Books.Remove(targetBook);

            try
            {
                context.SaveChanges();

                logicResponse.IsJobDone = true;
                return logicResponse;
            }
            catch(Exception ex)
            {
                logicResponse.Message = ex.Message;
                return logicResponse;
            }
        }

        public async Task<LogicResponse> UpdateAsync(BookDto bookDto)
        {
            LogicResponse logicResponse = new();

            var targetBook = context.Books.FirstOrDefault(B => B.Code == bookDto.Code);
            
            if(targetBook == null)
            {
                logicResponse.Message = "There is no Book with that Code";
                return logicResponse;
            }

            targetBook.Title = bookDto.Title;
            targetBook.Author = bookDto.Author;
            targetBook.Category = bookDto.Category;
            targetBook.Count = bookDto.Count;
            
            try
            {
                context.SaveChanges();

                logicResponse.IsJobDone = true;
                return logicResponse;
            }
            catch(Exception ex)
            {
                logicResponse.Message = ex.Message;
                return logicResponse;
            }
        }
    }
}
