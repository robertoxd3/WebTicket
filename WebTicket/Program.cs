using Microsoft.EntityFrameworkCore;
using WebTicket.Concrete;
using WebTicket.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<DatabaseContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DatabaseConnection")));

builder.Services.AddControllers();
builder.Services.AddScoped<ITicket, ApiTicketConcrete>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors(option =>
{
    option.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
});

app.UseHttpsRedirection();


app.UseAuthorization();

app.MapControllers();

app.Run();
