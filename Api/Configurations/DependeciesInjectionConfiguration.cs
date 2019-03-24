using BusinessLogic.Dtos;
using BusinessLogic.Services.Base;
using BusinessLogic.Services.Interfaces;
using DataAccess.Context;
using DataAccess.Repositories.Base;
using DataAccess.Repositories.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Configurations
{
    public static class DependeciesInjectionConfiguration
    {
        public static void AddRepositories(this IServiceCollection services) {
            services.AddScoped<IRepository<Book, int>, Repository<BookStoreContext, Book, int>>();
        }

        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IService<BookDto, int>, Service<BookDto, Book, int>>();
        }
    }
}
