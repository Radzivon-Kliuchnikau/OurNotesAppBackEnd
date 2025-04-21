using Microsoft.EntityFrameworkCore;
using OurNotesAppBackEnd.Interfaces;
using OurNotesAppBackEnd.Models;

namespace OurNotesAppBackEnd.Data.Repositories;

public class ProductRepository(ApplicationDbContext context) : BaseRepository<Product, Guid>(context), IProductRepository
{
    private readonly ApplicationDbContext _context = context;

    public new async Task<IEnumerable<Product>> GetAllEntitiesAsync()
    {
        return await _context.Set<Product>()
            .Include(p => p.Comments)
            .OrderByDescending(n => EF.Property<int>(n, "Id"))
            .AsNoTracking()
            .ToListAsync();
    }
    
    public new async Task<Product?> GetEntityByIdAsync(int id)
    {
        return await _context.Set<Product>()
            .Include(p => p.Comments)
            .FirstOrDefaultAsync(n => EF.Property<int>(n, "Id").Equals(id));
    }
};