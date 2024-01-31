using BookCatalogApi.Model;

public interface IBookRepository
{
    Task<IEnumerable<Book>> GetBooksAsync();
    Task<Book> GetBookByIdAsync(int id);
    Task<IEnumerable<Book>> GetBooksByCategoryIdAsync(int categoryId);
}
