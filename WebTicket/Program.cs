using Microsoft.EntityFrameworkCore;
using WebTicket.Concrete;
using WebTicket.Interface;
using WebTicket.Hubs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<DatabaseContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DatabaseConnection")));

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("pruebasecreta")),
        ValidateAudience = false,
        ValidateIssuer = false,
        ClockSkew = TimeSpan.Zero
    };
});


builder.Services.AddControllers();
//Concretes
builder.Services.AddScoped<ITicket, ApiTicketConcrete>();
builder.Services.AddScoped<IJwtGenerate, ApiJwtConcrete>();
builder.Services.AddScoped<IUsuarios, ApiUsuarioConcrete>();

builder.Services.AddSignalR(o =>
{
    o.EnableDetailedErrors = true;
    o.MaximumReceiveMessageSize = 10240; // bytes
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}


app.UseCors(option =>
{
    option.WithOrigins("*").AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
});

app.UseHttpsRedirection();


app.UseAuthorization();
app.MapHub<ColaHub>("/Cola");
app.MapControllers();
//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllers();
//    endpoints.MapHub<Cola>("/Cola");
//});
//app.UseWebSockets();



app.Run();
