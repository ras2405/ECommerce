using DataAccess.Context;
using DataAccess.Repositories;
using Logic;
using LogicInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RepositoryInterface;

namespace ServiceFactory
{
    public static class ServiceFactory
    {
        public static void AddServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IProductRepository, ProductRepository>();
            serviceCollection.AddScoped<IProductLogic, ProductLogic>();
            serviceCollection.AddScoped<IUserRepository, UserRepository>();
            serviceCollection.AddScoped<IUserLogic, UserLogic>();
            serviceCollection.AddScoped<IPurchaseLogic, PurchaseLogic>();
            serviceCollection.AddScoped<IPurchaseRepository, PurchaseRepository>();
            serviceCollection.AddScoped<ISessionLogic, SessionLogic>();
            serviceCollection.AddScoped<ISessionRepository, SessionRepository>();
            serviceCollection.AddScoped<IPromotionLogic, PromotionLogic>();
        }

        public static void AddConnectionString(this IServiceCollection serviceCollection, string connectionString)
        {
            serviceCollection.AddDbContext<DbContext, ECommerceContext>(o => o.UseSqlServer(connectionString));
        }
    }
}