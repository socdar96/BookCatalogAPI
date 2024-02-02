using BookCatalogApi.Data;
using BookCatalogApi.Request.Books.Commands.UpdateBook;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Data;

public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, bool>
{
    private readonly AppDbContext _dbContext;
    private readonly IMemoryCache _cache;

    public UpdateBookCommandHandler(AppDbContext dbContext, IMemoryCache cache)
    {
        _dbContext = dbContext;
        _cache = cache;
    }

    public async Task<bool> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var existingBook = await _dbContext.Books.FindAsync(request.Id);

            if (existingBook == null)
            {
                throw new NotFoundException($"Book with ID {request.Id} not found.");
            }

            existingBook.Title = request.Title;
            existingBook.Description = request.Description;
            existingBook.CategoryId = request.CategoryId;
            existingBook.PublishDateUtc = request.PublishDateUtc;

            await _dbContext.SaveChangesAsync();

            var cacheKey = $"Book_{existingBook.Id}";
            _cache.Set(cacheKey, existingBook, new MemoryCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromMinutes(30)
            });

            return true;
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new ConcurrencyException("Concurrency conflict. Please try again.");
        }
        catch (DbUpdateException ex)
        {
            throw new UpdateException($"Error updating book: {ex.Message}");
        }
        catch (Exception ex)
        {
            throw new InternalServerException($"Internal server error: {ex.Message}");
        }
    }
}

