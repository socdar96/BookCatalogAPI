using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBookRepository _bookRepository;

    public BooksController(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    [HttpGet]
    public async Task<IEnumerable<BookDto>> GetBooks()
    {
        var books = await _bookRepository.GetBooksAsync();
        return books.Select(book => new BookDto(book.Id, book.CategoryId, book.Title, book.Description, book.PublishDateUtc));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BookDto>> GetBookById(int id)
    {
        var book = await _bookRepository.GetBookByIdAsync(id);

        if (book == null)
        {
            return NotFound();
        }

        return new BookDto(book.Id, book.CategoryId, book.Title, book.Description, book.PublishDateUtc);
    }

    [HttpGet("category/{categoryId}")]
    public async Task<IEnumerable<BookDto>> GetBooksByCategoryId(int categoryId)
    {
        var books = await _bookRepository.GetBooksByCategoryIdAsync(categoryId);
        return books.Select(book => new BookDto(book.Id, book.CategoryId, book.Title, book.Description, book.PublishDateUtc));
    }
}
