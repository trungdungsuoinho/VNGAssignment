using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VNGAssignment.Models;
using VNGAssignment.Services;

namespace VNGAssignment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController(IBookService bookService) : ControllerBase
    {
        private readonly IBookService _bookService = bookService;

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var response = await _bookService.GetById(id);
            if (response != null)
            {
                return Ok(response);
            }
            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _bookService.GetAll();
            return Ok(response);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string? q)
        {
            var response = await _bookService.Search(new SearchBookRequest() { SearchText = q.Trim() });
            return Ok(response);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddBookRequest model)
        {
            var response = await _bookService.Create(model);
            return Ok(response);
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateBookRequest model)
        {
            var response = await _bookService.Update(model);
            if (response)
            {
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var response = await _bookService.Delete(id);
            if (response)
            {
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }
    }
}
