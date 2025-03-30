using Microsoft.EntityFrameworkCore;
using OurNotesAppBackEnd.Data;
using OurNotesAppBackEnd.Services;

namespace OurNotesAppBackEnd.Extensions;

public static class NoteServicesInstaller
{
    public static IServiceCollection AddNoteServicesInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<INotesService, NotesService>();
        
        services.AddDbContext<NotesAppDbContext>(options =>
        {
            options.UseSqlServer(configuration["ConnectionStrings:OurNotesConnection"]);
        });

        return services;
    }

    internal static void ApplyMigration(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
        using var context = serviceScope.ServiceProvider.GetService<NotesAppDbContext>();
        context?.Database.Migrate();
    }
}