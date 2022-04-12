using Microsoft.EntityFrameworkCore;
using DataAccess.EFCore;
using Mapping;
using Domain.Interfaces;
using DataAccess.EFCore.Repositories;
using DataAccess.EFCore.UnitOfWork;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Serilog;
using Trade.Interfaces;
using Trade.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Trade.Extenstions;

namespace Trade
{
    public static class ServicesConfiguration
    {
        public static readonly string CorsAllowAllInDevelopment = "CorsAllowAllInDeveloping";

        public static void ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.Host.UseSerilog((ctx, lc) => lc
                .WriteTo.Console()
                .WriteTo.Seq(builder.Configuration.GetConnectionString("SeqServerUrl"))
            );

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDataAccess(builder.Configuration);

            builder.Services.AddAutoMapper(typeof(MappingProfile));

            builder.Services.AddCors(DefinePolicies());

            builder.Services.AddApplicationServices();

            builder.Services.AddIdentity(builder.Configuration);

        }

        private static Action<CorsOptions> DefinePolicies()
        {
            return options =>
            {
                options.AddPolicy(CorsAllowAllInDevelopment, policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200", "https://localhost:4200");
                });
            };
        }
    }
}
