using Microsoft.EntityFrameworkCore;
using BidditApi.Models;
using BidditApi.Data;
using BidditApi.Utils;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(opt => opt.UseSqlServer(connectionString));
builder.Services.Configure<FormOptions>(options =>
{
    options.ValueCountLimit = int.MaxValue; ;
    options.ValueLengthLimit = int.MaxValue;
    options.MultipartBodyLengthLimit = long.MaxValue;
    options.MemoryBufferThreshold = Int32.MaxValue;
});
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

//app.Use(async (context, next) =>
//{
//    if (context.Request.HasFormContentType && context.Request.Form.Files.Any())
//    {
//        var formValueProvider = await context.Request.ReadFormAsync();
//        var formFile = formValueProvider.Files.FirstOrDefault();
//        if (formFile != null && formFile.Length > 0)
//        {
//            // Increase the limit to 100 MB
//            var multipartBodyLengthLimit = 100 * 1024 * 1024;
//            context.Features.Get<IHttpMaxRequestBodySizeFeature>().MaxRequestBodySize = multipartBodyLengthLimit;
//        }
//    }

//    await next();
//});


app.Use(async (context, next) =>
{
    Console.WriteLine(context.Request.Path);
    Console.WriteLine("eval"+ !context.Request.Path.StartsWithSegments("/api/Arts/getFile"));

    if (!context.Request.Path.StartsWithSegments("/api/Users")&&!context.Request.Path.StartsWithSegments("/api/Arts/getFile"))
    {
        Console.WriteLine("Inside");
        if (context.Request.Headers.ContainsKey("Authorization"))
        {
            var bearerToken = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var (isValid, value) = JWTHasher.ValidateToken(bearerToken);
            if (isValid)
            {
                context.Items["UserId"] = value;
                Console.WriteLine(value);

            }
            else
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Invalid");
                return;
            }
        }
        else
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Invalid");
            return;
        }
    }

    await next.Invoke();
});



app.MapControllers();

app.Run();
