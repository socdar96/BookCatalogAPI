using BookCatalogApi.Dtos;
using MediatR;

namespace BookCatalogApi.Commands.CreateBook
{
    public record CreateBookCommand(BookDto BookDto) : IRequest<int>;
}
