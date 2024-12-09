using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OurNotesAppBackEnd.Identity;
using OurNotesAppBackEnd.Utils;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "OurNotesFrontEnd",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173")
                .AllowAnyHeader()
                .AllowCredentials()
                .WithMethods("POST", "GET");
        });
});

builder.Services.AddDbContext<ApplicationIdentityDbContext>(options =>
    options.UseSqlServer(builder.Configuration["ConnectionStrings:OurNotesConnection"]));

builder.Services.AddAuthorization();

builder.Services.AddIdentityApiEndpoints<AppUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// builder.Services.AddAuthentication()
    // .AddCookie(IdentityConstants.ApplicationScheme);

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