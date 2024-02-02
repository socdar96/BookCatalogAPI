// GetBookQueryHandler.cs

using System.Threading;
using System.Threading.Tasks;
using BookCatalogApi.Models;
using BookCatalogApi.Repositories;
using BookCatalogApi.Request.Books.Queries.GetBooks;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace BookCatalogApi.Request.Books.Queries.GetBook
{
    public class GetBookQueryHandler : IRequestHandler<GetBookQuery, Book>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMemoryCache _cache;

        public GetBookQueryHandler(IBookRepository bookRepository, IMemoryCache cache)
        {
            _bookRepository = bookRepository;
            _cache = cache;
        }

        public async Task<Book> Handle(GetBookQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"Book_{request.BookId}";

            if (_cache.TryGetValue(cacheKey, out Book bookFromCache))
            {
                return bookFromCache;
            }

            var book = await _bookRepository.GetBookByIdAsync(request.BookId);

            if (book != null)
            {
                _cache.Set(cacheKey, book, new MemoryCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromMinutes(30)
                });
            }

            return book;
        }
    }
}
