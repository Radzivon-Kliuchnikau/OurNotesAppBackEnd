using Microsoft.EntityFrameworkCore;
using OurNotesAppBackEnd.Interfaces;
using OurNotesAppBackEnd.Models;
using OurNotesAppBackEnd.Utils;

namespace OurNotesAppBackEnd.Data.Repositories;

public class ProductRepository(ApplicationDbContext context) : BaseRepository<Product, Guid>(context), IProductRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<IEnumerable<Product>> GetAllEntitiesAsync(ProductQueryObject? productQuery)
    {
        var products = 
            _context.Set<Product>()
            .Include(p => p.Comments)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(productQuery?.Brand))
        {
            products = products.Where(p => p.Brand.Contains(productQuery.Brand));
        }

        if (!string.IsNullOrWhiteSpace(productQuery?.SortBy))
        {
            if (productQuery.SortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
            {
                products = productQuery.IsDecsending
                    ? products.OrderByDescending(p => p.Name)
                    : products.OrderBy(p => p.Name);
            }
        }

        var skipNumber = (productQuery.PageNumber - 1) * productQuery.PageSize;

        return await products.Skip(skipNumber).Take(productQuery.PageSize).ToListAsync();
    }
    
    public new async Task<Product?> GetEntityByIdAsync(Guid id)
    {
        return await _context.Set<Product>()
            .Include(p => p.Comments)
            .FirstOrDefaultAsync(n => EF.Property<int>(n, "Id").Equals(id));
    }
};