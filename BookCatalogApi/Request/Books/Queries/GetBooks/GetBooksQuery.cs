using BookCatalogApi.Models;
using MediatR;

namespace BookCatalogApi.Request.Books.Queries.GetBooks
{
    public class GetBooksQuery : IRequest<IEnumerable<Book>>
    {
        public int CategoryId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
