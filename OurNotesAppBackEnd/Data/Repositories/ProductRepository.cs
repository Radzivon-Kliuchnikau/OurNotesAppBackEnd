using Microsoft.EntityFrameworkCore;
using OurNotesAppBackEnd.Interfaces;
using OurNotesAppBackEnd.Models;

namespace OurNotesAppBackEnd.Data.Repositories;

public class ProductRepository(ApplicationDbContext context) : BaseRepository<Product, int>(context), IProductRepository;