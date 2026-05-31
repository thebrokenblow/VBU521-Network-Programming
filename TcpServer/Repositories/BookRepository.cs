using Core;
using Microsoft.EntityFrameworkCore;
using TcpServer.Persistence;

namespace TcpServer.Repositories;

public class BookRepository
{
    private readonly AppDbContext _context;

    public BookRepository()
    {
        _context = new AppDbContext();
    }

    public async Task AddAsync(Book book)
    {
        await _context.AddAsync(book);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Book>> GetAllAsync()
    {
        var books = await _context.Books.ToListAsync();

        return books;
    }

    public async Task RemoveAsync(int id)
    {
        var book = await GetByIdAsync(id);

        _context.Remove(book);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Book book)
    {
        var updatedBook = await GetByIdAsync(book.Id);

        updatedBook.Name = book.Name;
        updatedBook.Author = book.Author;
        updatedBook.Description = book.Description;

        await _context.SaveChangesAsync();
    }

    public async Task<Book> GetByIdAsync(int id)
    {
        var book = await _context.Books.FirstOrDefaultAsync(x => x.Id == id);

        return book;
    }
}
