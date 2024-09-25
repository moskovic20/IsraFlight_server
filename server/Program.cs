using ServicesGatway.Models;
using ServicesGatway.Services;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// הוספת השירותים שלנו
builder.Services.AddScoped<AirplaneService>();
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<FlightService>();
builder.Services.AddScoped<TicketService>();
builder.Services.AddScoped<ImaggaService>();
builder.Services.AddScoped<ShabbatTimeService>();
builder.Services.AddScoped<ParashaService>();

// הוספת ApplicationDbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// הוספת HttpClient
builder.Services.AddHttpClient();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
