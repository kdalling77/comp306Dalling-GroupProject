using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using _301247589_301276375_bright_aid_API.Models;
using _301247589_301276375_bright_aid_API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Register the StudentRepository & DonorRepository
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IDonorRepository, DonorRepository>();

// Register AutoMapper with the DI container
builder.Services.AddAutoMapper(typeof(Program));  // Or use the assembly where your profiles are defined

//builder.Services.AddDbContext<StudentContext>(opt => opt.UseInMemoryDatabase("StudentList"));
builder.Services.AddDbContext<StudentContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<DonorContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();
