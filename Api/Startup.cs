using Api.Configurations;
using AutoMapper;
using BusinessLogic.MappingProfiles;
using DataAccess.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ApiCore
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddCors(opt =>
            {
                opt.AddPolicy("EmployeeApp", policy =>
                {
                    policy.WithOrigins("http://localhost:4000");
                    policy.AllowAnyOrigin();
                    policy.AllowAnyHeader();
                    policy.AllowAnyMethod();
                });
            });

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new BookProfile());
            });

          
            services.AddAutoMapper();
            services.AddRepositories();
            services.AddServices();

            services.AddDbContext<BookStoreContext>(opt => {
                opt.UseSqlServer(Configuration.GetConnectionString("BookStoreDb"), m => m.MigrationsAssembly("BookStore"));
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors("EmployeeApp");
            app.UseMvc();
            
           
        }
    }
}
