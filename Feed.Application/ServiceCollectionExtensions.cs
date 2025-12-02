using Feed.Application.Interfaces;
using Feed.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Feed.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBLL(this IServiceCollection services)
    {
        services.AddScoped<IPoolService, PoolService>();
        services.AddScoped<IVoteService, VoteService>();
        services.AddScoped<IPoolOptionService, PoolOptionService>();
        return services;
    }
}
