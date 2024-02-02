using BookCatalogApi.Commands.CreateBook;
using BookCatalogApi.Dtos;
using BookCatalogApi.Models;
using BookCatalogApi.Request.Books.Commands.CreateBook;
using BookCatalogApi.Request.Books.Commands.DeleteBook;
using BookCatalogApi.Request.Books.Commands.UpdateBook;
using BookCatalogApi.Request.Books.Queries.GetAllBooks;
using BookCatalogApi.Request.Books.Queries.GetBook;
using BookCatalogApi.Request.Books.Queries.GetBooks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace BookCatalogApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMemoryCache _cache;

        public BooksController(IMediator mediator, IMemoryCache cache)
        {
            _mediator = mediator;
            _cache = cache;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBookById(int id)
        {
            var book = await _mediator.Send(new GetBookQuery { BookId = id });

            if (book != null)
            {
                return Ok(book);
            }

            return NotFound();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            var allBooksFromCache = await _cache.GetOrCreateAsync("AllBooks", async entry =>
            {
                var result = await _mediator.Send(new GetAllBooksQuery());

                if (result.Any())
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(30);
                    return result;
                }

                return Enumerable.Empty<Book>();
            });

            return allBooksFromCache.Any() ? Ok(allBooksFromCache) : NotFound();
        }

        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<Book>>> FilterBooks([FromQuery] GetBooksQuery query)
        {
            var filteredBooks = await _mediator.Send(query);

            if (filteredBooks != null && filteredBooks.Any())
            {
                return Ok(filteredBooks);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateBook([FromBody] CreateBookCommand command)
        {
            var bookId = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetBooks), new { id = bookId }, bookId);
        }

        [HttpPut]
        public async Task<ActionResult<bool>> UpdateBook([FromBody] UpdateBookCommand command)
        {
            var result = await _mediator.Send(command);

            if (result)
            {
                _cache.Remove("AllBooks");

                return Ok();
            }

            return NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteBook(int id)
        {
            var command = new DeleteBookCommand { BookId = id };
            var result = await _mediator.Send(command);

            if (result)
            {
                return Ok(true);
            }

            return NotFound();
        }
    }
}
