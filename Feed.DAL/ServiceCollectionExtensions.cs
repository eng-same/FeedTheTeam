using Feed.Domain.Interfaces;
using Feed.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Feed.Domain;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDAL(this IServiceCollection services)
    {
        //regster repositories 
        services.AddScoped<IVotesRepository, VotesRepository>();
        services.AddScoped<IPoolRepository,PoolRepository>();
        services.AddScoped<IPoolOptionsRepository, PoolOptionsRepository>();
        return services;
    }
}
