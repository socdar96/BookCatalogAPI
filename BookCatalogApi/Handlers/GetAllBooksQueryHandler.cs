using BookCatalogApi.Models;
using BookCatalogApi.Queries;
using BookCatalogApi.Repositories;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

public class GetAllBooksQueryHandler : IRequestHandler<GetAllBooksQuery, IEnumerable<Book>>
{
    private readonly IBookRepository _bookRepository;
    private readonly IMemoryCache _cache;

    public GetAllBooksQueryHandler(IBookRepository bookRepository, IMemoryCache cache)
    {
        _bookRepository = bookRepository;
        _cache = cache;
    }

    public async Task<IEnumerable<Book>> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
    {
        if (_cache.TryGetValue("AllBooks", out IEnumerable<Book>? allBooksFromCache))
        {
            return allBooksFromCache;
        }

        var books = await _bookRepository.GetAllAsync();

        _cache.Set("AllBooks", books, new MemoryCacheEntryOptions
        {
            SlidingExpiration = TimeSpan.FromMinutes(30)
        });

        return books.ToList();
    }
}
