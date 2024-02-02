using BookCatalogApi.Models;
using BookCatalogApi.Repositories;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace BookCatalogApi.Request.Books.Queries.GetBooks
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
            // Check if all books are in cache
            if (_cache.TryGetValue("AllBooks", out IEnumerable<Book>? allBooksFromCache))
            {
                // Filter the cached data based on the request criteria
                var filteredBooks = FilterBooksFromCache(allBooksFromCache, request.CategoryId, request.PageNumber, request.PageSize);
                if (filteredBooks.Any())
                {
                    return filteredBooks;
                }
            }

            // If not found or filtered result is empty, query the repository for all books
            var allBooks = await _bookRepository.GetAllAsync();

            // Cache all books with sliding expiration
            var allBooksCacheEntryOptions = new MemoryCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromMinutes(30)
            };
            _cache.Set("AllBooks", allBooks, allBooksCacheEntryOptions);

            // Filter the result based on the request criteria
            var filteredResult = FilterBooksFromCache(allBooks, request.CategoryId, request.PageNumber, request.PageSize);

            return filteredResult;
        }

        private IEnumerable<Book> FilterBooksFromCache(IEnumerable<Book> books, int categoryId, int pageNumber, int pageSize)
        {
            // Implement your filtering logic here based on the request criteria
            var filteredBooks = books.Where(b => b.CategoryId == categoryId)
                                    .Skip((pageNumber - 1) * pageSize)
                                    .Take(pageSize);

            return filteredBooks;
        }
    }
}
