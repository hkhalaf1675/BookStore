using DB_Assigment.Contexts;
using DB_Assigment.IRepositories;
using DB_Assigment.IRepository;
using DB_Assigment.Models;
using DB_Assigment.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// inject the dbcontext
var DefaultConnection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
            DefaultConnection,
            b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)
    ));

// inject the identity
builder.Services.AddIdentity<User,IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();

// inject the IRepositories
builder.Services.AddScoped(typeof(IAuthRepository), typeof(AuthRepository));
builder.Services.AddScoped(typeof(IBookRepository), typeof(BookRepository));
builder.Services.AddScoped(typeof(IBorrowingRespository), typeof(BorrowingRespository));

#region Authentication
builder.Services.AddAuthentication(Options =>
{
    Options.DefaultAuthenticateScheme = "Default";
    Options.DefaultChallengeScheme = "Default";
})
.AddJwtBearer("Default", options =>
{
    var KeyString = builder.Configuration.GetValue<string>("JWT:Key");
    var KeyInBytes = Encoding.ASCII.GetBytes(KeyString);
    var Key = new SymmetricSecurityKey(KeyInBytes);
    options.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = Key,
        ValidateIssuer = false,
        ValidateAudience = false
    };
});
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();