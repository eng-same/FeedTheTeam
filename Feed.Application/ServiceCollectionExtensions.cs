using Feed.Application.Commands.Account;
using Feed.Application.Commands.Pool;
using Feed.Application.Commands.PoolOptions;
using Feed.Application.Commands.Vote;
using Feed.Application.Interfaces;
using Feed.Application.Requests.Account;
using Feed.Application.Requests.Pool;
using Feed.Application.Requests.PoolOption;
using Feed.Application.Requests.Vote;
using Feed.Application.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Feed.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBLL(this IServiceCollection services)
    {
        services.AddScoped<IPoolService, PoolService>();
        services.AddScoped<IVoteService, VoteService>();
        services.AddScoped<IPoolOptionService, PoolOptionService>();
        services.AddScoped<IValidator<VoteRequest>, VoteRequestValidator>();
        services.AddScoped<IValidator<CreatePoolRequest>, CreatePoolValidator>();
        services.AddScoped<IValidator<CreatePoolOptionRequest>, CreatePoolOptionValidator>();
        services.AddScoped<IValidator<RegisterRequest>, RegisterValidator>();
        services.AddScoped<IValidator<LoginRequest>, LoginValidator>();
        return services;
    }
}
