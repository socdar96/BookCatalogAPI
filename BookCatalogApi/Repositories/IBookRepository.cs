using BookCatalogApi.Models;

namespace BookCatalogApi.Repositories
{
    public interface IBookRepository
    {
        IQueryable<Book> GetBooksFilteredAndPaged(int categoryId, int pageNumber, int pageSize);
        Task<IEnumerable<Book>> GetAllAsync();
    }
}
