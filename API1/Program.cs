using Microsoft.EntityFrameworkCore;
using API1.Models;

var builder = WebApplication.CreateBuilder(args);

var OpenMeUp = "_openMeUp";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: OpenMeUp,
        builder =>
        {
            //demo only. Not recommended for production.
            builder.WithOrigins("http://localhost:8080").AllowAnyMethod().AllowAnyHeader();

        });
});

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<ShoppingItemContext>(opt => opt.UseInMemoryDatabase("ShoppingList"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseCors(OpenMeUp);



app.UseAuthorization();

app.MapControllers();

app.Run();