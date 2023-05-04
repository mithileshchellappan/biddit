using Microsoft.EntityFrameworkCore;
using BidditApi.Models;
using BidditApi.Data;
using BidditApi.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(opt => opt.UseSqlServer(connectionString));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseCors(builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
});


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.Use(async (context, next) =>
{

    if (context.Request.Headers.ContainsKey("Authorization"))
    {
    var bearerToken = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var (isValid,value) =  JWTHasher.ValidateToken(bearerToken);
        if (isValid)
        {
            Console.WriteLine(value);

        }
    }

    await next.Invoke();
});

app.MapControllers();

app.Run();
