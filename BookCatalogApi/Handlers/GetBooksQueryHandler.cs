using BookCatalogApi.Models;
using BookCatalogApi.Queries;
using BookCatalogApi.Repositories;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace BookCatalogApi.Handlers
{
    public class GetBooksQueryHandler : IRequestHandler<GetBooksQuery, IEnumerable<Book>>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMemoryCache _cache;

        public GetBooksQueryHandler(IBookRepository bookRepository, IMemoryCache cache)
        {
            _bookRepository = bookRepository;
            _cache = cache;
        }

        public async Task<IEnumerable<Book>> Handle(GetBooksQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"FilteredBooks_{request.CategoryId}_{request.PageNumber}_{request.PageSize}";

            if (_cache.TryGetValue(cacheKey, out IEnumerable<Book>? filteredBooksFromCache))
            {
                return filteredBooksFromCache;
            }

            var books = _bookRepository.GetBooksFilteredAndPaged(request.CategoryId, request.PageNumber, request.PageSize);

            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromMinutes(30)
            };
            _cache.Set(cacheKey, books, cacheEntryOptions);

            return books.ToList();
        }
    }
}