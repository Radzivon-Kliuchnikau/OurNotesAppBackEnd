using OurNotesAppBackEnd.Data.Repositories;
using OurNotesAppBackEnd.Interfaces;
using OurNotesAppBackEnd.Services;

namespace OurNotesAppBackEnd.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddDependencyServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IBaseRepository<,>), typeof(BaseRepository<,>));
        services.AddScoped<INoteRepository, NoteRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICommentRepository, CommentRepository>();
        services.AddScoped<ITokenService, TokenService>();

        return services;
    } 
}