using ADO.API.MinimalAPI;
using ADO.API.Validator;
using Demo.Domain.Attributes;
using Demo.Infrastructure.Database;
using Demo.Infrastructure.Modules;
using FluentValidation;
using Microsoft.OpenApi.Models;
using System.Reflection.Metadata;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                     .AddEnvironmentVariables();

builder.Services.AddValidatorsFromAssemblyContaining<StudentCreatedDTOValidator>();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new CustomDateTimeConverter());
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

try
{
    // Register the endpoint mappings
    app.MapStudentEndpoints();
}
catch (Exception ex)
{
    // Log the exception and provide more details
    Console.WriteLine($"Exception occurred while adding endpoints: {ex.Message}");
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.Run();
