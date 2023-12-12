using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Configurations
{
    public interface IStartupApplication
    {
        IConfiguration Configuration { get; }

        void ConfigureServices(IServiceCollection services);

        void Configure(WebApplication app, IWebHostEnvironment env);        
    }
}