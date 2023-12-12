namespace api.Configurations;

public class Startup : IStartupApplication
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.ConfigurarServicos();
    }

    public void Configure(WebApplication app, IWebHostEnvironment env)
    {
        app.UsarServicos();
    }
}