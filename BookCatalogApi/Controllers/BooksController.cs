using BookCatalogApi.Commands;
using BookCatalogApi.Models;
using BookCatalogApi.Queries;
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            if (_cache.TryGetValue("AllBooks", out IEnumerable<Book>? allBooksFromCache))
            {
                return Ok(allBooksFromCache);
            }

            var query = new GetAllBooksQuery();
            allBooksFromCache = await _mediator.Send(query);

            if (allBooksFromCache.Any())
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromMinutes(30)
                };

                _cache.Set("AllBooks", allBooksFromCache, cacheEntryOptions);

                return Ok(allBooksFromCache);
            }

            return NotFound();
        }

        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<Book>>> FilterBooks([FromQuery] GetBooksQuery query)
        {
            var filteredBooks = await _mediator.Send(query);
            return Ok(filteredBooks);
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateBook([FromBody] CreateBookCommand command)
        {
            var bookId = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetBooks), new { id = bookId }, bookId);
        }
    }
}
