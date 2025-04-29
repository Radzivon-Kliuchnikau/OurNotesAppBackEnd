using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OurNotesAppBackEnd.Data;
using OurNotesAppBackEnd.Data.Repositories;
using OurNotesAppBackEnd.Extensions;
using OurNotesAppBackEnd.Identity;
using OurNotesAppBackEnd.Interfaces;
using OurNotesAppBackEnd.Models;
using OurNotesAppBackEnd.Utils;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
{
    loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration);
});

builder.Services.AddGeneralCors();
builder.Services.ConfigureCookie();

builder.Services
    .AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    });

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration["ConnectionStrings:OurNotesConnection"]);
});

// builder.Services.AddDbContext<ApplicationIdentityDbContext>(options =>
// {
//     options.UseSqlServer(builder.Configuration["ConnectionStrings:OurNotesConnection"]);
// });

// builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
//     {
//         options.Password.RequireDigit = true;
//         options.Password.RequireLowercase = true;
//         options.Password.RequireUppercase = true;
//         options.Password.RequireNonAlphanumeric = true;
//         options.Password.RequiredLength = 12;
//     })
//     .AddEntityFrameworkStores<ApplicationIdentityDbContext>();

// builder.Services.AddAuthentication(options =>
// {
//     options.DefaultAuthenticateScheme =
//         options.DefaultChallengeScheme =
//             options.DefaultForbidScheme =
//                 options.DefaultScheme =
//                     options.DefaultSignInScheme =
//                         options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
// }).AddJwtBearer(options =>
// {
//     options.TokenValidationParameters = new TokenValidationParameters
//     {
//         ValidateIssuer = true,
//         ValidIssuer = builder.Configuration["JWT:Issuer"],
//         ValidateAudience = true,
//         ValidAudience = builder.Configuration["JWT:Audience"],
//         ValidateIssuerSigningKey = true,
//         IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:SignInKey"]))
//     };
// });

// builder.Services.AddAuthorization();

// builder.Services.AddIdentityApiEndpoints<AppUser>()
//     .AddRoles<IdentityRole>()
//     .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
//     .AddDefaultTokenProviders();


builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped(typeof(IBaseRepository<,>), typeof(BaseRepository<,>));
builder.Services.AddScoped<INoteRepository, NoteRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();

var app = builder.Build();

// app.ApplyMigration();

app.UseSerilogRequestLogging();

app.ConfigureExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.MapIdentityApi<AppUser>();

app.UseHttpsRedirection();
app.UseCors("OurNotesFrontEnd"); // TODO: Specify CORS settings here

// app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// using (var scope = app.Services.CreateScope())
// {
//     var serviceProvider = scope.ServiceProvider;
//     await SeedDefaultData.CreateDefaultRoles(serviceProvider);
//     await SeedDefaultData.CreateDefaultAdminUser(serviceProvider, builder.Configuration);
// }

app.Run();