using MediatR;

namespace BookCatalogApi.Commands
{
    public record CreateBookCommand(int CategoryId, string Title, string Description, DateTime PublishDateUtc) : IRequest<int>;

}
