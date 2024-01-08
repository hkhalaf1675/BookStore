using DB_Assigment.DTOs;
using DB_Assigment.Models;

namespace DB_Assigment.Profiles
{
    public static class MappingBook
    {
        public static Book MapDtoToBook(BookDto bookDto)
        {
            return new Book
            {
                Code = bookDto.Code,
                Title = bookDto.Title,
                Author = bookDto.Author,
                Category = bookDto.Category,
                Count = bookDto.Count
            };
        }

        public static BookDto MapBookToDto(Book book)
        {
            return new BookDto
            {
                Code = book.Code,
                Title = book.Title,
                Author = book.Author,
                Category = book.Category,
                Count = book.Count
            };
        }

        public static UserBorrowedBookDto MapToBorrowedDto(BookDto book,DateTime borrowedDate)
        {
            return new UserBorrowedBookDto
            {
                Code = book.Code,
                Title = book.Title,
                BorrowedDate = borrowedDate
            };
        }

        public static BookBorrowingHistory MapToBookBorrowingHistory(BorrowingHistory history)
        {
            return new BookBorrowingHistory
            {
                Code = history.BookCode,
                Title = history.Book.Title,
                UserName = history.User.UserName,
                BorrowedDate = history.BorrowDate,
                ReturnDate = history.ReturnDate
            };
        }
    }
}
