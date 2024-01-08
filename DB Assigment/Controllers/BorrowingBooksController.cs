using DB_Assigment.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DB_Assigment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BorrowingBooksController : ControllerBase
    {
        private readonly IBorrowingRespository borrowingRespository;
        private readonly IBookRepository bookRepository;

        public BorrowingBooksController(IBorrowingRespository _borrowingRespository,IBookRepository _bookRepository)
        {
            borrowingRespository = _borrowingRespository;
            bookRepository = _bookRepository;
        }

        [HttpGet("get-borrowed-books")]
        public async Task<IActionResult> GetBorrowedBooks()
        {
            var userId = User.Claims.FirstOrDefault(C => C.Type == ClaimTypes.NameIdentifier)?.Value;
            if(string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var books = await borrowingRespository.GetUserBorrowedBooksAsync(userId);
            if(books == null)
            {
                return NotFound();
            }

            return Ok(books);
        }

        [HttpGet("borrow-book/{bookCode}")]
        public async Task<IActionResult> BorrowBook(string bookCode)
        {
            var userId = User.Claims.FirstOrDefault(C => C.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var response = await borrowingRespository.BorrowBookAync(userId, bookCode);

            if (response.IsJobDone)
            {
                return Ok();
            }

            return BadRequest(response.Message);
        }

        [HttpGet("unborrow-book/{bookCode}")]
        public async Task<IActionResult> UnBorrowBook(string bookCode)
        {
            var userId = User.Claims.FirstOrDefault(C => C.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var response = await borrowingRespository.UnBorrowBookAsync(userId, bookCode);

            if (response.IsJobDone)
            {
                return Ok();
            }

            return BadRequest(response.Message);
        }

        [HttpGet("book-borrowing-history/{bookCode}")]
        public async Task<IActionResult> GetBookBorrowingHistory(string bookCode)
        {
            var book = await bookRepository.GetAsync(bookCode);
            if(book == null)
            {
                return BadRequest("There is no Book with that Code");
            }

            var history = await borrowingRespository.GetBookBorrowingHistoryAsync(bookCode);

            if (history == null)
            {
                return NotFound();
            }

            return Ok(history);
        }
    }
}
