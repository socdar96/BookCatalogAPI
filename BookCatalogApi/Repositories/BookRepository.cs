using BookCatalogApi.Data;
using BookCatalogApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookCatalogApi.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly AppDbContext _dbContext;

        public BookRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<Book> GetBooksFilteredAndPaged(int categoryId, int pageNumber, int pageSize)
        {
            var query = _dbContext.Books.AsQueryable();

            query = categoryId >= 0 ? query.Where(book => book.CategoryId == categoryId) : query;

            return query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }

        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            return await _dbContext.Books.ToListAsync();
        }
    }
}
