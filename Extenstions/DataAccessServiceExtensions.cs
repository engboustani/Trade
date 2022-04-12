using DataAccess.EFCore;
using DataAccess.EFCore.Repositories;
using DataAccess.EFCore.UnitOfWork;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Trade.Extenstions
{
    public static class DataAccessServiceExtensions
    {
        public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<ApplicationContext>(
                opt => opt.UseSqlServer(
                    config.GetConnectionString("MSSqlConnection")
                    , b => b.MigrationsAssembly(typeof(ApplicationContext).Assembly.FullName)
                )
            );

            #region Repositories
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<ICurrencyRepository, CurrencyRepository>();
            services.AddTransient<ITransactionRepository, TransactionRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            #endregion
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
