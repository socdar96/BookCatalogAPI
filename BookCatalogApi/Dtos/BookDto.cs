namespace BookCatalogApi.Dtos
{
    public record BookDto(int Id, int CategoryId, string Title, string Description, DateTime PublishDateUtc);
}
