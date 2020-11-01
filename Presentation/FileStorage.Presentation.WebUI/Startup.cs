using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FileStorage.Layers.L00_BaseModels;
using FileStorage.Layers.L02_DataLayer;
using FileStorage.Layers.L03_Services;
using FileStorage.Layers.L03_Services.Contracts;
using FileStorage.Layers.L03_Services.Imps;
using FileStorage.Presentation.WebUI.TokenAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FileStorage.Presentation.WebUI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
           
            services.AddDbContext<FileStorageDbContext>(options =>
            {
                options.UseSqlServer(
                    Configuration.GetConnectionString("SqlServer:ApplicationDbContextConnection"));
            });
            services.AddHttpContextAccessor();
            
            services.AddTransient<ICustomAuthentication, CustomAuthentication>();
            services.AddTransient<IFileStorageRepo, FileStorageRepo>();
            services.AddTransient<IFileService, FileService>();
            services.Configure<Settings>(options => Configuration.Bind(options));
            services.AddAuthentication(o => {
                o.DefaultScheme = "TokenAuthenticationScheme";
            })
                .AddScheme<TokenAuthenticationOptions, TokenAuthenticationHandler>("TokenAuthenticationScheme", o => { });
            services.AddMemoryCache();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
