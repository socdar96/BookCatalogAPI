using BookCatalogApi.Commands.CreateBook;
using BookCatalogApi.Data;
using BookCatalogApi.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace BookCatalogApi.Request.Books.Commands.CreateBook
{
    public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, int>
    {
        private readonly AppDbContext _dbContext;
        private readonly IMemoryCache _cache;

        public CreateBookCommandHandler(AppDbContext dbContext, IMemoryCache cache)
        {
            _dbContext = dbContext;
            _cache = cache;
        }

        public async Task<int> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            var newBook = new Book
            {
                Title = request.BookDto.Title,
                Description = request.BookDto.Description,
                CategoryId = request.BookDto.CategoryId,
                PublishDateUtc = request.BookDto.PublishDateUtc
            };

            await _dbContext.Books.AddAsync(newBook);
            await _dbContext.SaveChangesAsync();

            var bookCacheKey = $"Book_{newBook.Id}";

            _cache.Set(bookCacheKey, newBook, new MemoryCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromMinutes(30)
            });

            if (!_cache.TryGetValue("AllBooks", out List<Book>? allBooksFromCache))
            {
                allBooksFromCache = new List<Book> { newBook };

                _cache.Set("AllBooks", allBooksFromCache, new MemoryCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromMinutes(30)
                });
            }
            else
            {
                allBooksFromCache.Add(newBook);

                _cache.Set("AllBooks", allBooksFromCache, new MemoryCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromMinutes(30)
                });
            }

            return newBook.Id;
        }
    }
}
