// GetBookQuery.cs

using MediatR;
using BookCatalogApi.Models;

namespace BookCatalogApi.Request.Books.Queries.GetBooks
{
    public class GetBookQuery : IRequest<Book>
    {
        public int BookId { get; set; }

        public GetBookQuery()
        {
        }

        public GetBookQuery(int bookId)
        {
            BookId = bookId;
        }
    }
}
