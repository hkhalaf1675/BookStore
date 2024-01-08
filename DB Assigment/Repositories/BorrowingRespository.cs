using DB_Assigment.Contexts;
using DB_Assigment.DTOs;
using DB_Assigment.IRepositories;
using DB_Assigment.Models;
using DB_Assigment.Profiles;
using Microsoft.EntityFrameworkCore;

namespace DB_Assigment.Repositories
{
    public class BorrowingRespository : IBorrowingRespository
    {
        private readonly ApplicationDbContext context;
        private readonly IBookRepository bookRepository;

        public BorrowingRespository(ApplicationDbContext context,IBookRepository bookRepository)
        {
            this.context = context;
            this.bookRepository = bookRepository;
        }
        public async Task<LogicResponse> BorrowBookAync(string userId, string bookCode)
        {
            LogicResponse logicResponse = new ();

            var targetBook = await bookRepository.GetAsync(bookCode);

            if (targetBook == null )
            {
                logicResponse.Message = "There is no book with that Code";
                return logicResponse;
            }

            // check if there is items of the book
            if(targetBook?.Count == 0)
            {
                logicResponse.Message = "There is no available items of that book";
                return logicResponse;
            }

            var borrowedBook = context.BorrowingHistories.FirstOrDefault(B => B.UserId == userId &&  B.BookCode == bookCode && B.ReturnDate == null);

            // check if the user already borrowed that book or not
            if(borrowedBook != null)
            {
                logicResponse.Message = "the user already borrowed that book";
                return logicResponse;
            }

            context.BorrowingHistories.Add(new Models.BorrowingHistory
            {
                UserId = userId,
                BookCode = bookCode,
                BorrowDate = DateTime.UtcNow,
            });

            // decrease the count of book
            targetBook.Count--;

            var response = await bookRepository.UpdateAsync(targetBook);

            if(!response.IsJobDone) 
            {
                return response;
            }

            try
            {
                context.SaveChanges();

                logicResponse.IsJobDone = true;
                return logicResponse;
            }
            catch (Exception ex)
            {
                return new LogicResponse
                {
                    IsJobDone = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<ICollection<UserBorrowedBookDto>> GetUserBorrowedBooksAsync(string userId)
        {
            ICollection<UserBorrowedBookDto> borrowedBooksDto = new List<UserBorrowedBookDto>();

            var borrowedBooks = context.BorrowingHistories.Where(B => B.UserId == userId && B.ReturnDate == null).ToList();
            foreach (var borrowedBook in borrowedBooks)
            {
                var book = await bookRepository.GetAsync(borrowedBook.BookCode);
                if(book != null)
                {
                    borrowedBooksDto.Add(MappingBook.MapToBorrowedDto(book, borrowedBook.BorrowDate));
                }
            }

            return borrowedBooksDto;
        }

        public async Task<LogicResponse> UnBorrowBookAsync(string userId, string bookCode)
        {
            LogicResponse logicResponse = new();

            var targetBook = await bookRepository.GetAsync(bookCode);

            if(targetBook == null)
            {
                logicResponse.Message = "There is no books with that Code";
                return logicResponse;
            }

            var borrowedBook = context.BorrowingHistories.FirstOrDefault(B => B.UserId == userId && B.BookCode == bookCode && B.ReturnDate == null);

            // check if the user borrowed the book or not
            if(borrowedBook == null)
            {
                logicResponse.Message = "That User did not borrow that Book";
                return logicResponse;
            }

            // increate the count of the book
            targetBook.Count++;

            var response = await bookRepository.UpdateAsync(targetBook);

            if(!response.IsJobDone)
            {
                return response;
            }

            // update the return date
            borrowedBook.ReturnDate = DateTime.UtcNow;

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

        public async Task<ICollection<BookBorrowingHistory>> GetBookBorrowingHistoryAsync(string bookCode)
        {
            ICollection<BookBorrowingHistory> borrowingHistory = new List<BookBorrowingHistory>();

            var histories = context.BorrowingHistories.Include(B => B.User).Include(B => B.Book).Where(B => B.BookCode == bookCode).ToList();

            foreach(var history in histories)
            {
                borrowingHistory.Add(MappingBook.MapToBookBorrowingHistory(history));
            }

            return borrowingHistory;
        }
    }
}
