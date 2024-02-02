using MediatR;

namespace BookCatalogApi.Request.Books.Commands.DeleteBook
{
    public class DeleteBookCommand : IRequest<bool>
    {
        public int BookId { get; set; }
    }
}
