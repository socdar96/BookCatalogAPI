using BookCatalogApi.Models;
using MediatR;

namespace BookCatalogApi.Queries
{
    public record GetAllBooksQuery : IRequest<IEnumerable<Book>>;
}
