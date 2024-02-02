using BookCatalogApi.Models;
using MediatR;

namespace BookCatalogApi.Queries
{
    public class GetBooksQuery : IRequest<IEnumerable<Book>>
    {
        public int CategoryId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
