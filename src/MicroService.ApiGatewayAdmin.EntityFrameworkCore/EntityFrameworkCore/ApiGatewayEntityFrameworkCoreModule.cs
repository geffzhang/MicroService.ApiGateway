﻿using MicroService.ApiGateway.Data.Filter;
using MicroService.ApiGateway.Entites.Ocelot;
using MicroService.ApiGateway.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Data;
using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement.EntityFrameworkCore;

namespace MicroService.ApiGateway.EntityFrameworkCore
{
    [DependsOn(
        typeof(ApiGatewayDomainModule),
        typeof(AbpSettingManagementEntityFrameworkCoreModule)
        )]
    public class ApiGatewayEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<DataFilterOptions>(options =>
            {
                options.DefaultStates.Add(typeof(IActivation), new DataFilterState(true));
            });

            context.Services.AddAbpDbContext<ApiGatewayDbContext>(options =>
            {
                //Remove "includeAllEntities: true" to create default repositories only for aggregate roots
                options.AddRepository<ReRoute, IReRouteRepository>();
                options.AddRepository<GlobalConfiguration, IGlobalConfigRepository>();
                options.AddRepository<DynamicReRoute, IDynamicReRouteRepository>();
                options.AddRepository<AggregateReRoute, IAggregateReRouteRepository>();

                options.Entity<ReRoute>(opt =>
                    opt.DefaultWithDetailsFunc = (p) => 
                        p.Include(q => q.AuthenticationOptions)
                         .Include(q => q.CacheOptions)
                         .Include(q => q.HttpHandlerOptions)
                         .Include(q => q.LoadBalancerOptions)
                         .Include(q => q.QoSOptions)
                         .Include(q => q.RateLimitOptions)
                         .Include(q => q.SecurityOptions));

                options.Entity<GlobalConfiguration>(opt =>
                    opt.DefaultWithDetailsFunc = (p) =>
                        p.Include(q => q.HttpHandlerOptions)
                         .Include(q => q.LoadBalancerOptions)
                         .Include(q => q.QoSOptions)
                         .Include(q => q.RateLimitOptions)
                         .Include(q => q.ServiceDiscoveryProvider));

                options.Entity<DynamicReRoute>(opt =>
                    opt.DefaultWithDetailsFunc = (p) =>
                        p.Include(q => q.RateLimitRule));

                options.AddDefaultRepositories(includeAllEntities: true);
            });
        }
    }
}
