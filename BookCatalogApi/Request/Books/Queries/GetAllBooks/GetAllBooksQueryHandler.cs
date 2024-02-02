using BookCatalogApi.Models;
using BookCatalogApi.Repositories;
using BookCatalogApi.Request.Books.Queries.GetAllBooks;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

public class GetAllBooksQueryHandler : IRequestHandler<GetAllBooksQuery, IEnumerable<Book>>
{
    private readonly IBookRepository _bookRepository;

    public GetAllBooksQueryHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<IEnumerable<Book>> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
    {
        var result = await _bookRepository.GetAllAsync();
        return result.Any() ? result : Enumerable.Empty<Book>();
    }
}
