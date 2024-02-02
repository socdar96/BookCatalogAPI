using BookCatalogApi.Commands;
using BookCatalogApi.Data;
using BookCatalogApi.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace BookCatalogApi.Handlers
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
                Title = request.Title,
                Description = request.Description,
                CategoryId = request.CategoryId,
                PublishDateUtc = request.PublishDateUtc
            };

            await _dbContext.Books.AddAsync(newBook);
            await _dbContext.SaveChangesAsync();

            _cache.Remove("AllBooks");

            _cache.Set($"Book_{newBook.Id}", newBook, new MemoryCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromMinutes(30)
            });

            return newBook.Id;
        }
    }
}
