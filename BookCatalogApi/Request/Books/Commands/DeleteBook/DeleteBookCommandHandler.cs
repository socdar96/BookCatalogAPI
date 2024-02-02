using BookCatalogApi.Data;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace BookCatalogApi.Request.Books.Commands.DeleteBook
{
    public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand, bool>
    {
        private readonly AppDbContext _dbContext;
        private readonly IMemoryCache _cache;

        public DeleteBookCommandHandler(AppDbContext dbContext, IMemoryCache cache)
        {
            _dbContext = dbContext;
            _cache = cache;
        }

        public async Task<bool> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var bookToDelete = await _dbContext.Books.FindAsync(request.BookId);

                if (bookToDelete == null)
                {
                    throw new NotFoundException($"Book with ID {request.BookId} not found.");
                }

                _dbContext.Books.Remove(bookToDelete);
                await _dbContext.SaveChangesAsync();

                var cacheKey = $"Book_{bookToDelete.Id}";
                _cache.Remove(cacheKey);

                return true;
            }
            catch (Exception ex)
            {
                throw new InternalServerException($"Error deleting book: {ex.Message}");
            }
        }
    }
}
