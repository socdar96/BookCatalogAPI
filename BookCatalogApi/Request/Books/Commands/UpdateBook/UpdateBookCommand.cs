using MediatR;

namespace BookCatalogApi.Request.Books.Commands.UpdateBook
{
    public class UpdateBookCommand : IRequest<bool>
    {
        public int Id { get; set; }        
        public int CategoryId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; } 
        public DateTime PublishDateUtc { get; set; }
    }
}
