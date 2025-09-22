using Amazon.DynamoDBv2;
using Amazon.S3;
using Canais.Application.Service;
using Canais.Domain.Contracts.Repositories;
using Canais.Application.Interfaces;
using Canais.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Canais.Application;

public static class ApplicationModule
{
    public static IServiceCollection AddApplicationDependency(this IServiceCollection services)
    {
        services.AddServices();

        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        //var config = new AmazonDynamoDBConfig
        //{
        //    RegionEndpoint = Amazon.RegionEndpoint.USEast1
        //};

        services.AddScoped<IReclamacaoService, ReclamacaoService>();
        services.AddScoped<ISqsService, SqsService>();
        services.AddScoped<IReclamacaoRepository, ReclamacaoRepository>();
        services.AddAWSService<IAmazonS3>();
        services.AddScoped<IBucketService, BucketService>();
        services.AddScoped<IHistoricoClienteProvider, HistoricoClienteProviderService>();
        //services.AddScoped<IAmazonDynamoDB>(sp =>
        //{
        //    var config = new AmazonDynamoDBConfig
        //    {
        //        RegionEndpoint = Amazon.RegionEndpoint.USEast1
        //    };

        //    return new AmazonDynamoDBClient(config);
        //});

        return services;
    }
}
