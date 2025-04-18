using Microsoft.EntityFrameworkCore;
using OurNotesAppBackEnd.Interfaces;
using OurNotesAppBackEnd.Models;

namespace OurNotesAppBackEnd.Data.Repository;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;

    public ProductRepository(ApplicationDbContext context)
    {
        _context = context;
    }


    public async Task<IEnumerable<Product>> GetAllEntitiesAsync()
    {
        return await _context.Products.ToListAsync();
    }

    public async Task<Product?> GetEntityByIdAsync(int id)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

        return product;
    }

    public async Task AddEntityAsync(Product entity)
    {
        await _context.Products.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task EditEntity(Product entity)
    {
        _context.Products.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteEntity(Product entity)
    {
        _context.Products.Remove(entity);
        await _context.SaveChangesAsync();
    }
}