using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OurNotesAppBackEnd.Models;

namespace OurNotesAppBackEnd.Data;

public class ApplicationDbContext : IdentityDbContext<AppUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    public DbSet<Note> Notes { get; init; }

    public DbSet<NoteAccesses> NoteAccesses { get; set; }

    public DbSet<Product> Products { get; init; }

    public DbSet<Comment> Comments { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        modelBuilder.Entity<Note>()
            .HasOne(n => n.AppUser)
            .WithMany(a => a.Notes)
            .HasForeignKey(n => n.AppUserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<NoteAccesses>()
            .HasKey(na => new { na.NoteId, na.AppUserId });
        
        modelBuilder.Entity<NoteAccesses>()
            .HasOne(na => na.Note)
            .WithMany(n => n.NoteAccesses)
            .HasForeignKey(na => na.NoteId);
        
        modelBuilder.Entity<NoteAccesses>()
            .HasOne(na => na.AppUser)
            .WithMany(a => a.NoteAccesses)
            .HasForeignKey(na => na.AppUserId);
        
        
        modelBuilder.Entity<Comment>()
            .HasOne(c => c.Product)
            .WithMany(p => p.Comments)
            .HasForeignKey(c => c.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
        
        var identityRoles = new List<IdentityRole>
        {
            new()
            {
                Id = "0821E65C-724B-4DC7-88DC-8E02775A4100", 
                ConcurrencyStamp = "0821E65C-724B-4DC7-88DC-8E02775A4100", 
                Name = "Admin", 
                NormalizedName = "ADMIN"
            },
            new()
            {
                Id = "6C4EE28F-7B94-4011-A0C0-6B08EFBEEA25", 
                ConcurrencyStamp = "6C4EE28F-7B94-4011-A0C0-6B08EFBEEA25", 
                Name = "User", 
                NormalizedName = "USER"
            }
        };
        
        modelBuilder.Entity<IdentityRole>()
            .HasData(identityRoles);
    }
}