using DB_Assigment.DTOs;
using DB_Assigment.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DB_Assigment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository bookRepository;

        public BooksController(IBookRepository _bookRepository)
        {
            bookRepository = _bookRepository;
        }

        [HttpGet("get-by-id/{code}")]
        public async Task<IActionResult> GetById(string code)
        {
            var book = await bookRepository.GetAsync(code);

            if (book == null)
            {
                return NotFound("There is no Book with that Code");
            }

            return Ok(book);
        }

        [HttpPost("add-new")]
        public async Task<IActionResult> AddNew(BookDto bookDto)
        {
            var response = await bookRepository.AddNewAsync(bookDto);

            if (response.IsJobDone)
            {
                return Ok();
            }

            return BadRequest(response.Message);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update(BookDto bookDto)
        {
            var response = await bookRepository.UpdateAsync(bookDto);

            if(response.IsJobDone)
            {
                return Ok();
            }

            return BadRequest(response?.Message);
        }

        [HttpDelete("delete/{code}")]
        public async Task<IActionResult> Delete(string code)
        {
            var response  =await bookRepository.DeleteAsync(code);

            if (response.IsJobDone)
            {
                return Ok();
            }

            return BadRequest(response.Message);
        }
    }
}
