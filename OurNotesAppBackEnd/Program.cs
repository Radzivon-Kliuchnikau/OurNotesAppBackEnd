using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OurNotesAppBackEnd.Data;
using OurNotesAppBackEnd.Data.Repository;
using OurNotesAppBackEnd.Identity;
using OurNotesAppBackEnd.Models;
using OurNotesAppBackEnd.Services;
using OurNotesAppBackEnd.Utils;

var builder = WebApplication.CreateBuilder(args);

Console.WriteLine("builder.Configuration[\"MongoDBConnectionURI\"] " + builder.Configuration["MongoDBConnectionURI"]);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "OurNotesFrontEnd",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.Name = ".AspNetCore.OurNotes.Identity";
});

var mongoDbSettings = builder.Configuration.GetSection("MongoDbSettings").Get<MongoDBSettings>();
builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDbSettings"));

builder.Services.AddDbContext<NotesAppDbContext>(options =>
{
    options.UseMongoDB(builder.Configuration["MongoDBConnectionURI"] ?? "", mongoDbSettings?.DatabaseName ?? "");
});

builder.Services.AddScoped<INoteMongoDbRepository, NoteMongoDbRepository>();
builder.Services.AddScoped<INotesService, NotesService>();

builder.Services.AddDbContext<ApplicationIdentityDbContext>(options =>
    options.UseSqlServer(builder.Configuration["ConnectionStrings:OurNotesConnection"]));

builder.Services.AddAuthorization();

builder.Services.AddIdentityApiEndpoints<AppUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddControllers();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapIdentityApi<AppUser>();

app.UseHttpsRedirection();
app.UseCors("OurNotesFrontEnd");

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    await SeedDefaultData.CreateDefaultRoles(serviceProvider);
    await SeedDefaultData.CreateDefaultAdminUser(serviceProvider, builder.Configuration);
}

app.Run();