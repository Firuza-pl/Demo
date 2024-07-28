using Demo.Infrastructure.Repositories;
using Demo.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.Infrastructure.Modules;
public class LogicModule
{
    public static void Load(IServiceCollection services)
    {
        services.AddScoped<IStudentService, StudentService>();

        //repos
        services.AddScoped<IStudentRepository, StudentRepository>();
    }
}
