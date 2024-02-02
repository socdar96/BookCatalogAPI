using BookCatalogApi.Models;
using MediatR;

namespace BookCatalogApi.Request.Books.Queries.GetAllBooks
{
    public record GetAllBooksQuery : IRequest<IEnumerable<Book>>;
}
