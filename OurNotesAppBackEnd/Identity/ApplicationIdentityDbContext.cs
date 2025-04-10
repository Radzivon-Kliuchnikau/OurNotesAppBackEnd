using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace OurNotesAppBackEnd.Identity;

public class ApplicationIdentityDbContext(DbContextOptions<ApplicationIdentityDbContext> options)
    : IdentityDbContext<AppUser>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
};