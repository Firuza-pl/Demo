using ADO.API.MinimalAPI;
using ADO.API.Validator;
using ADO.API.Validator.Course;
using Demo.Domain.Attributes;
using Demo.Infrastructure.Database;
using Demo.Infrastructure.Modules;
using FluentValidation;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                     .AddEnvironmentVariables();


builder.Services.AddValidators();  //from static ValidatorConfigurationClass

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});

builder.Services.AddSingleton(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("DefaultConnection");
    return new AdoNetDbContext(connectionString);
});

//register services
LogicModule.Load(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

try
{
    // Register the endpoint mappings
    app.MapStudentEndpoints();
    app.MapCourseEndpoints();

}
catch (Exception ex)
{
    // Log the exception and provide more details
    Console.WriteLine($"Exception occurred while adding endpoints: {ex.Message}");
}

app.Run();
